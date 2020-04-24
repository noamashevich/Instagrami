import logging
from DB import ServerDB
from protocol import MessageType, HEADER_FMT
import struct
import os
from datetime import datetime
from hashlib import sha224

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
		elif msg_type == MessageType.IMAGE_UPLOAD:
			return self.handle_image_upload(message)


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


	def handle_image_upload(self, message):
		"""
		Handles an image upload message

		@param message The message to handle

		@note Image upload message content:
			  [username_size: 10 bytes]
			  [text_size: 10 bytes]
			  [image_size: 10 bytes]
			  [username: ~username_size~ bytes]
			  [text: ~text_size~ bytes]
			  [image: ~image_size~ bytes]

		@return bytes The header of the message to return
		"""
		SIZE_FIELD_LEN = 10
		USERNAME_SIZE_END = TXT_SIZE_START = SIZE_FIELD_LEN
		TXT_SIZE_END = IMG_SIZE_START = 2 * SIZE_FIELD_LEN
		IMG_SIZE_END = USERNAME_START = 3 * SIZE_FIELD_LEN

		try:
			username_size = int(message[:USERNAME_SIZE_END])
			text_size = int(message[TXT_SIZE_START:TXT_SIZE_END])
			image_size = int(message[IMG_SIZE_START:IMG_SIZE_END])
		except Execption as e:
			logging.exception(e)
			return self.build_header(0, MessageType.INVAL_MSG_FMT)

		username_end = text_start = USERNAME_START + username_size
		username = message[USERNAME_START:username_end]

		text_end = image_start = text_start + text_size
		text = message[text_start:text_end]

		image_end = image_start + image_size
		image = message[image_start:image_end]

		if len(username) != username_size or \
		   len(text) != text_size or \
		   len(image) != image_size:
			logging.error('The sizes don\'t match')
			logging.info(f'username: {username}, {len(username)} : {username_size}')
			logging.info(f'text: {text}, {len(text)} : {text_size}')
			logging.info(f'image: {len(image)} : {image_size}')
			return self.build_header(0, MessageType.INVAL_MSG_FMT)

		date_time_now = datetime.now()
		time = str(datetime.time(date_time_now))
		date = str(datetime.date(date_time_now))

		date_time_str = date + time
		date_time_str = date_time_str.replace('-', '').replace(':', '').replace('.', '')

		image_hash = sha224(image + text).hexdigest()

		image_path = os.path.join(username.decode('ascii'), image_hash + date_time_str)
		image_path = os.path.abspath(image_path)

		if not os.path.exists(username):
			os.mkdir(username)

		with open(image_path, "wb") as image_file:
			image_file.write(image)

		if not self.server_db.add_image(username, text, image_path):
			logging.error(f'Failed to upload image for user {username}')
			return self.build_header(0, MessageType.NO_SUCH_USER)
		
		return self.build_header(0, MessageType.IMG_UPLOAD_SUCCESS)