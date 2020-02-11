#!/usr/bin/python

import socket 
import select
import logging
import struct

from protocol import *

class Server():
	def __init__(self, port):
		self.s = socket.socket()
		self.s.bind(("0.0.0.0", port))
		self.s.listen(5)
		self.clients = []

	def run(self):
		self.handle_sockets()

	
	def accept_client(self):
		conn, addr = self.s.accept()
		logging.warning('Client connected')
		self.clients.append(conn)


	def client_send(self, client, message, message_type):
		message_bytes = bytearray(message, encoding='ascii')

		length = len(message_bytes)
		user_id = 0
		message_type = message_type

		header = struct.pack(HEADER_FMT, length, user_id, message_type)

		client.send(header + message_bytes)


	def client_recv(self, client):
		header_bytes = bytearray(client.recv(HEADER_LENGTH))

		if not header_bytes:
			self.handle_client_disconnect(client)
			return None, None

		header = struct.unpack(HEADER_FMT, header_bytes)

		message = client.recv(header[0])

		return header, message


	def handle_client(self, client):
		header, message = self.client_recv(client)

		if message is None:
			return

		logging.warning('Client sent: {}:{}'.format(repr(header), repr(message)))

		self.client_send(client, "WOW from server", MessageType.MAIN_PAGE)


	def handle_client_disconnect(self, client):
		logging.warning('Client {} disconnected!'.format(client))
		self.clients.remove(client)


	def remove_bad_clients(self):
		for client in self.clients:
			try:
				_, _, _ = select.select([client], [], [], 0)
			except:
				self.clients.remove(client)


	def handle_sockets(self):
		while True:
			try:
				readable, _, disconnected = select.select(self.clients + [self.s], [], self.clients)

				logging.error(readable)
				readable = filter(lambda x: x not in disconnected, readable)
				logging.error(readable)

				for sock in readable:
					if sock is self.s:
						self.accept_client()
					else:
						self.handle_client(sock)

				for sock in disconnected:
					self.handle_client_disconnect(sock)
			except ConnectionResetError as e:
				self.remove_bad_clients()
				

def main():
	try:
		server = Server(7014)
		server.run()
	except Exception as e:
		print(e)
	finally:
		input()

if __name__ == '__main__':
	main()