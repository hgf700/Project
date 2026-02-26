from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from services.auth_service import verify_internal_key
from db.get_db import get_db
from db.model import UserMediaStatuses
from db.Dto import movieTagsFromASP
from services.recommendation_service import recommendation_process

router = APIRouter(
    prefix="/recommend",
    tags=["recommendations"]
)

@router.post("/start-recommend-process-py")
async def start_recommend_process(request: movieTagsFromASP):

    print("REQUEST:", request)
    print("TAGS:", request.tags)

    results = recommendation_process(request.tags)

    return {
        "status": "ok",
        "recommendations": results
    }