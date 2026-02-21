from fastapi import Header, HTTPException
import os

INTERNAL_API_KEY = os.getenv("JWT_SECRET")

def verify_internal_key(x_internal_key: str = Header(None)):
    if x_internal_key != INTERNAL_API_KEY:
        raise HTTPException(status_code=403, detail="Unauthorized")