import logging
import sqlite3
from hashlib import sha224

class ServerDB():
	# Constants
	DB_NAME = 'InstagramiDB'

	'''
	SQL commands
	'''
	# Table creation
	CREATE_USERS_TABLE = '''CREATE TABLE IF NOT EXISTS users (
							username TEXT PRIMARY KEY NOT NULL,
							password_hash TEXT NOT NULL)'''

	CREATE_IMAGES_TABLE = '''CREATE TABLE IF NOT EXISTS images (
							 image_location TEXT PRIMARY KEY NOT NULL,
							 image_text TEXT NOT NULL,
							 username TEXT NOT NULL
							 likes INT UNSIGNED NOT NULL)'''

	# Table manipulation
	GET_ALL_USERS = '''SELECT * FROM users'''
	GET_USER_BY_USERNAME = GET_ALL_USERS + ''' WHERE username="{}"'''	
	GET_ALL_USERS_EXCEPT = GET_ALL_USERS + ''' WHERE username<>"{}"'''

	ADD_USER = '''INSERT INTO users VALUES ("{}", "{}")'''

	GET_ALL_IMAGES = '''SELECT * FROM images ORDER BY image_location'''
	GET_ALL_IMAGES_EXCEPT = '''SELECT * FROM images WHERE username<>"{}" ORDER BY image_location'''

	ADD_IMAGE = '''INSERT INTO images VALUES ("{}", "{}", "{}", 0)'''

	def __init__(self):
		"""
		The DB's constructor.
		Initializes the DB and creates the tables if they don't exist.
		"""
		self.conn = sqlite3.connect(ServerDB.DB_NAME)

		self.conn.execute(ServerDB.CREATE_USERS_TABLE)
		self.conn.execute(ServerDB.CREATE_IMAGES_TABLE)


	def get_user_by_username(self, username):
		"""
		Gets a user by it's username.

		@param username The username to search the user by.

		@return list Returns a list of the users found this can be only a list with one entry,
					 if the user exists, or an empty list if the user doesn't exist.
		"""
		res = self.conn.execute(ServerDB.GET_USER_BY_USERNAME.format(username)).fetchall()
		logging.debug(res)
		return res


	def get_all_users(self):
		return self.conn.execute(ServerDB.GET_ALL_USERS).fetchall()


	def get_all_users_except(self, username):
		return self.conn.execute(ServerDB.GET_ALL_USERS_EXCEPT.format(username)).fetchall()


	def add_user(self, username, password):
		"""
		Adds a user to the users table.
		Note that *password* itself isn't saved in the DB, we save only the
		sha224 of the password in the table so getting the data in the DB doesn't
		reveal the passwords.

		@param username The username of the user to add.
		@param password The password of the user to add.

		@return bool Returns True if the user was added successfully and False otherwise.
		"""
		if self.get_user_by_username(username):
			return False

		pass_hash = sha224(password).hexdigest()
		
		logging.debug(f'Added {username}, {password}, {pass_hash} to the DB')

		self.conn.execute(ServerDB.ADD_USER.format(username, pass_hash))

		# Update the DB on disk
		self.conn.commit()

		return True


	def get_all_images(self):
		return self.conn.execute(ServerDB.GET_ALL_IMAGES).fetchall()


	def get_all_images_except(self, user_to_ignore):
		logging.info(f'{ServerDB.GET_ALL_IMAGES_EXCEPT.format(user_to_ignore)}')
		return self.conn.execute(ServerDB.GET_ALL_IMAGES_EXCEPT.format(user_to_ignore)).fetchall()


	def add_image(self, username, image_txt, image_path):
		if not self.get_user_by_username(username):
			logging.info(f'Non existent user {username} tried to add photo with text {image_txt}')
			return False

		self.conn.execute(ServerDB.ADD_IMAGE.format(image_path, image_txt, username))

		# Update the DB on disk
		self.conn.commit()
		logging.info(f'User {username} added photo with text {image_txt}')

		return True

	def get_images(self, user_to_ignore, start, amount):
		images = self.get_all_images_except(user_to_ignore)

		return images # [start:start+amount]


	def update_image(self, image_row):
		pass


	def like_image(self, image):
		image_row = self.get_image(image)

		image_row[3] += 1

		self.update_image(image_row)