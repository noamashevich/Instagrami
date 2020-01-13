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
		logging.warning('Client sent: {}'.format(client.recv(4096)))

	def handle_sockets(self):
		while True:
			readable, _, _ = select.select(self.clients + [self.s], [], [])
			for sock in readable:
				if sock == self.s:
					self.accept_client()
				else:
					self.handle_client(sock)

def main():
	server = Server(7000)
	server.run()
	input()

if __name__ == '__main__':
	main()