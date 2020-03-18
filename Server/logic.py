import logging
from DB import ServerDB
from protocol import MessageType, HEADER_FMT
import struct

class Logic(object):
	def __init__(self):
		self.server_db = ServerDB()


	def build_header(self, length, type):
		return struct.pack(HEADER_FMT, length, 0, type)


	def handle_client_message(self, header, message):
		msg_type = header[2]

		if msg_type == MessageType.SIGN_UP:
			return self.handle_sign_up(message)
		elif msg_type == MessageType.SIGN_IN:
			return self.handle_sign_in(message)


	def handle_sign_up(self, message):
		username, password = message.split(bytearray(', ', encoding='ascii'))

		if self.server_db.add_user(username, password):
			logging.info('Added user {} successfully'.format(username))
			return self.build_header(0, MessageType.SIGN_UP_SUCCESS)
		else:
			logging.info('User {} already exists'.format(username))
			return self.build_header(0, MessageType.SIGN_UP_EXISTS)

	def handle_sign_in(self, message):
		username, password = message.split(bytearray(', ', encoding='ascii'))

		tryUsername = self.server_db.get_user_by_username(username)
		if len(tryUsername) != 0:
			if tryUsername[0][1] == sha224(password).hexdigest():
				return self.build_header(0, MessageType.SIGN_IN_SUCCESS)
		
		return self.build_header(0, MessageType.SIGN_IN_FAIL)