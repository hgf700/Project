from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import pandas as pd

csv_path = r'C:\Users\USER098\Documents\GitHub\Project\RecommendationsML\Data\results\conected_data.csv'
df = pd.read_csv(csv_path)

cv = TfidfVectorizer(
    max_features=20000,
    stop_words='english',
    ngram_range=(1,2)
)

vectors = cv.fit_transform(df['tags']).toarray()

tag="Action ScienceFiction Thriller ТимурБекмамбетов ChrisPratt RebeccaFerguson KaliReis AtlasEntertainment AmazonMGMStudios Bazelevs In the near future, a detective stands on trial accused of murdering his wife. He has ninety minutes to prove his innocence to the advanced AI Judge he once championed, before it determines his fate. 2026"

def recommend(tag):
    tag_vector= cv.transform([tag])

    similarity = cosine_similarity(tag_vector,vectors)


    distances = list(enumerate(similarity[tag]))
    distances = sorted(distances, key=lambda x: x[1], reverse=True)[1:6]  
    for i in distances:
        print(df.iloc[i[0]]['title'])

recommend(tag)
