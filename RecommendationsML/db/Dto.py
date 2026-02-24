from typing import List
from pydantic import BaseModel

class getMovieRequestDto(BaseModel):
    movieId: List[int]