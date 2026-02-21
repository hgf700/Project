from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from services.auth_service import verify_internal_key
from db.get_db import get_db
from db.model import UserMediaStatuses

router = APIRouter(
    prefix="/recommend",
    tags=["recommendations"]
)

@router.get("/{user_id}")
def recommend(user_id: int,
              db: Session = Depends(get_db),
              _: str = Depends(verify_internal_key)):

    liked = db.query(UserMediaStatuses.MovieId)\
               .filter(
                   (UserMediaStatuses.UserId == user_id) &
                   (UserMediaStatuses.Rating == 2)
               ).all()

    liked_ids = [row[0] for row in liked]

    return {"liked_movies": liked_ids}