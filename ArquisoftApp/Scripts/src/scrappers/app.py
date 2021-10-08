import requests
import configparser
import os
from bs4 import BeautifulSoup
import pandas as pd

config_path = os.path.join(os.path.abspath(os.path.dirname(__file__)), "config.ini")
config = configparser.ConfigParser()
config.read(config_path)

BASE_URL = config['epa_config']['base_url']
PAGES = config['epa_config']['endpoints'].split(";")
USER_AGENT = config['app_config']['user_agent']
CSV_PATH = config['app_config']['csv_path']
SEPARATOR = config['app_config']['separator']



def get_html(page):
    url = BASE_URL+page+".html"
    headers = {"User-Agent": USER_AGENT}
    page = requests.get(url, headers=headers)
    soup = BeautifulSoup(page.content, 'html.parser')

    return soup

def process_data():
    dataset = []
    headings = ["description", "url", "image", "price"]

    for page in PAGES:
        page_content = get_html(page)
        print("Processing page:" +page)

        products_data = page_content.find_all("li",{"class":"item product product-item"})


    for prod in products_data:
        img = prod.find("img", {"class": "product-image-photo"})['src']
        desc = prod.find("a",{"class":"product-item-link"}).get_text()
        url = prod.find("a",{"class":"product-item-link"})['href']
        prc = prod.find("span",{"class":"precio"}).get_text()
        
        dataset.append([desc, url, img, prc])

    df = pd.DataFrame(dataset, columns = headings)
    return df


df = process_data()
df.to_csv(CSV_PATH, sep=SEPARATOR, index=False, encoding='utf-8-sig')