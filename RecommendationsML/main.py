from fastapi import FastAPI, HTTPException, Header
from fastapi.middleware.cors import CORSMiddleware
from routers.recommend_router import router as recommend_router
import uvicorn

app = FastAPI()

origins = [
    "http://localhost:4200", 
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(recommend_router)

if __name__ == "__main__":
    uvicorn.run(app, host="localhost", port=5000)