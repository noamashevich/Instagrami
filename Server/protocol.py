from enum import IntEnum

HEADER_LENGTH = 12
HEADER_FMT = '@LLl'

class MessageType(IntEnum):
	SIGN_IN        = 0
	SIGN_UP        = 1
	IMAGE_UPLOAD   = 2
	IMAGE_DOWNLOAD = 3
	MAIN_PAGE      = 4