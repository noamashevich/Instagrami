#!/usr/bin/python

import socket 
import select
import logging
import struct

from logic import Logic
from protocol import *

class Server():
	"""
	This class is responsible for the network and protocol logic.
	It is used to accept and handle clients and their requests.
	"""

	def __init__(self, port):
		"""
		The server's constructor

		@param port The port the server will listen on
		"""
		self.s = socket.socket()
		self.s.bind(("0.0.0.0", port))
		self.s.listen(5)
		self.clients = []
		self.server_logic = Logic()


	def run(self):
		"""
		Starts the server.
		The server will listen for connections and handle them from now on.
		This method blocks unless an unhandled exception rises.
		"""
		self.handle_sockets()

	
	def accept_client(self):
		"""
		Accepts a client to the server.
		If a client isn't waiting to be accepted the method will block.
		"""
		conn, addr = self.s.accept()
		logging.info(f'Client {addr[0]} connected')
		self.clients.append(conn)


	def client_send(self, client, reply):
		"""
		Builds the header of the message based on the *message_type* and
		sends the *message* with the header to the *client*.
		
		@param client The client we will send the message to
		@param reply The reply to send to the client
		"""
		client.send(reply)


	def client_recv(self, client):
		"""
		Recieves a message from the client.

		@param client The client to recieve a message from

		@return (bytearray, bytearray) Returns the tuple (header, message) on successful parsing
									   and returns (None, None) on parsing / network error.
		"""
		try:
			header_bytes = bytearray(client.recv(HEADER_LENGTH))

			if not header_bytes:
				return None, None

			header = struct.unpack(HEADER_FMT, header_bytes)
			message = client.recv(header[0])
		except Exception as e:
			logging.exception(f'Client message caused: {e}')
			return None, None

		return header, message


	def handle_client(self, client):
		"""
		Handles a client's request, which might send the client a response.

		@param client The client to handle a request for.
		"""
		header, message = self.client_recv(client)

		if message is None or header is None:
			logging.info(f'disconnecting client {client}')
			self.handle_client_disconnect(client)
			return

		logging.debug('Client sent: {}|{}'.format(repr(header), repr(message)))

		reply = self.server_logic.handle_client_message(header, message)

		logging.debug(f'reply: {reply}')

		self.client_send(client, reply)


	def handle_client_disconnect(self, client):
		"""
		Handles the disconnection of *client* - this includes removing it from the list
		of clients and closing the socket.
		"""
		logging.info('Client {} disconnected!'.format(client))
		self.clients.remove(client)
		client.close()


	def handle_sockets(self):
		"""
		Handles all of the sockets - that includes the server socket that recieves
		new clients, and the clients themselves.

		This is a blocking method and will not return unless there is an unhandeld exception.
		"""
		while True:
			readable, _, disconnected = select.select(self.clients + [self.s], [], self.clients)

			for sock in disconnected:
				self.handle_client_disconnect(sock)

			for sock in readable:
				if sock is self.s:
					self.accept_client()
				else:
					try:
						self.handle_client(sock)
					except Exception as e:
						logging.exception(f'Client caused exception {e}')
						self.handle_client_disconnect(sock)


def main():
	logging.basicConfig(level=logging.INFO)

	try:
		server = Server(7019)
		server.run()
	except Exception as e:
		logging.exception(e)
	finally:
		input('Server crashed\nPress any key to continue...')

if __name__ == '__main__':
	main()