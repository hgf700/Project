from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from RecommendationsML.temp.auth_service import verify_internal_key
# from db.get_db import get_db
# from db.model import UserMediaStatuses
from db.Dto import movieTagsFromASP
from services.recommendation_service import recommendation_process

router = APIRouter(
    prefix="/recommendations",
    tags=["recommendations"]
)

@router.post("/receive-recommend-process-py")
async def start_recommend_process(request: movieTagsFromASP):

    results = recommendation_process(request.tags)

    print(results)

    return {
        "recommendations": results
    }