#!/usr/bin/python

import socket 
import select
import logging
import struct

HEADER_LENGTH = 12
HEADER_FMT = '@LLl'

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

	def handle_client(self, client):
		header_bytes = bytearray(client.recv(HEADER_LENGTH))

		if not header_bytes:
			self.handle_client_disconnect(client)

		header = struct.unpack(HEADER_FMT, header_bytes)

		print header

		message = client.recv(header[0])
		logging.warning('Client sent: {}'.format(repr(message)))

	def handle_client_disconnect(self, client):
		logging.warning('Client {} disconnected!'.format(client))
		self.clients.remove(client)		

	def handle_sockets(self):
		while True:
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


def main():
	try:
		server = Server(7000)
		server.run()
	except Exception as e:
		print(e)
	finally:
		input()

if __name__ == '__main__':
	main()