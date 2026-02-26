from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import pandas as pd

csv_path = r'C:\Users\USER098\Documents\GitHub\Project\RecommendationsML\Data\results\final.csv'

df = pd.read_csv(csv_path)

cv = TfidfVectorizer(
    max_features=20000,
    stop_words='english',
    ngram_range=(1,2)
)

def recommendation_process(tag_array):

    recommendations = []
    
    vectors = cv.fit_transform(df['tags']).toarray()

    for tag_a in tag_array:

        tag_vector= cv.transform([tag_a])

        similarity = cosine_similarity(tag_vector,vectors)

        distances = list(enumerate(similarity[0]))
        distances = sorted(distances, key=lambda x: x[1], reverse=True)[1:6]  
        for i in distances:
            recommendations.append(df.iloc[i[0]]['title'])

    return recommendations