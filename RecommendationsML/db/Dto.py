from typing import List
from pydantic import BaseModel

class movieTagsFromASP(BaseModel):
    tags: List[str]