from enum import IntEnum, unique

HEADER_LENGTH = 12
HEADER_FMT = '@LLl'

@unique
class MessageType(IntEnum):
	SIGN_IN            = 0
	SIGN_UP            = 1
	IMAGE_UPLOAD       = 2
	IMAGE_DOWNLOAD     = 3
	REQUEST_MAIN_PAGE  = 4
	RESPONSE_MAIN_PAGE = 5
	SIGN_UP_SUCCESS    = 6
	SIGN_UP_EXISTS     = 7
	SIGN_IN_SUCCESS    = 8
	SIGN_IN_FAIL       = 9
	INVAL_MSG_FMT      = 10
	NO_SUCH_USER       = 11
	IMG_UPLOAD_SUCCESS = 12	
	IMG_UPLOAD_FAIL    = 13
	IMG_LIKE	       = 14