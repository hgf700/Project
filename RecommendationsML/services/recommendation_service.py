from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import pandas as pd

csv_path = r'C:\Users\USER098\Documents\GitHub\Project\RecommendationsML\Data\results\id_title_tag.csv'

df = pd.read_csv(csv_path)

cv = TfidfVectorizer(
    max_features=20000,
    stop_words='english',
    ngram_range=(1,2)
)

vectors = cv.fit_transform(df['tags'])

def recommendation_process(tag_array):
    tag_array = tag_array.lower()

    query = " ".join(tag_array)

    query_vector = cv.transform([query])

    similarity = cosine_similarity(query_vector, vectors).flatten()

    top_idx = similarity.argsort()[::-1][1:6]

    recommendations = [(df.iloc[i]['title'], similarity[i]) for i in top_idx]

    return recommendations

    