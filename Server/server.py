#!/usr/bin/python

import socket 
class Server():
	def __init__(self, port):
		self.s = socket.socket()
		self.s.bind(("0.0.0.0", port))
		self.s.listen(5)

	def run(self):
		conn, addr = self.s.accept()
		print '{} connected'.format(addr[0])

def main():
	server = Server(7000)
	server.run()
	input()

if __name__ == '__main__':
	main()