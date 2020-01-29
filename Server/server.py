#!/usr/bin/python

import socket 
import select
import logging

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
		message = client.recv(4096)
		logging.warning('Client sent: {}'.format(repr(message)))
		if not message:
			self.handle_client_disconnect(client)

	def handle_client_disconnect(self, client):
		logging.warning('Client {} disconnected!'.format(client))
		self.clients.remove(client)		

	def handle_sockets(self):
		while True:
			readable, _, disconnected = select.select(self.clients + [self.s], [], self.clients)

			readable = filter(lambda x: x not in disconnected, readable)

			for sock in readable:
				if sock is self.s:
					self.accept_client()
				else:
					self.handle_client(sock)

			for sock in disconnected:
				self.handle_client_disconnect(sock)


def main():
	server = Server(7000)
	server.run()
	input()

if __name__ == '__main__':
	main()