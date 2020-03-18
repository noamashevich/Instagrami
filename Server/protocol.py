from enum import IntEnum

HEADER_LENGTH = 12
HEADER_FMT = '@LLl'

class MessageType(IntEnum):
	SIGN_IN         = 0
	SIGN_UP         = 1
	IMAGE_UPLOAD    = 2
	IMAGE_DOWNLOAD  = 3
	MAIN_PAGE       = 4
	SIGN_UP_SUCCESS = 5
	SIGN_UP_EXISTS  = 6
	SIGN_IN_SUCCESS = 7
	SIGN_IN_FAIL    = 8