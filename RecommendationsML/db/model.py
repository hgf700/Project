from sqlalchemy import Column, Integer, String, DateTime
from db.database import Base

class UserMediaStatuses(Base):
    __tablename__ = "UserMediaStatuses"

    Id = Column(Integer, primary_key=True, index=True)
    UserId = Column(String)
    MovieId = Column(Integer)
    Rating = Column(Integer)
    CreatedAt = Column(DateTime)

class Genres(Base):
    __tablename__ = "Genres"

    Id = Column(Integer, primary_key=True, index=True)
    TmdbId = Column(Integer)
    Name = Column(String)