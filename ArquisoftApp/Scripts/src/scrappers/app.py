import requests
import configparser
import os
import pyodbc
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



def get_html(page, first_iteration):
    if(first_iteration == True):
        url = BASE_URL+page+".html"
    else:
        url = page
    headers = {"User-Agent": USER_AGENT}
    page = requests.get(url, headers=headers)
    soup = BeautifulSoup(page.content, 'html.parser')

    return soup

def process_data():
    dataset = []
    headings = ["description", "url", "image", "price"]

    for page in PAGES:
        print("Processing page:" +page)
        page_content = get_html(page, True)
        
        while True:
            products_data = page_content.find_all("li",{"class":"item product product-item"})
            next_page = page_content.find("a",{"class":"action next"})

            for prod in products_data:
                img = prod.find("img", {"class": "product-image-photo"})['src']
                desc = prod.find("a",{"class":"product-item-link"}).get_text()
                url = prod.find("a",{"class":"product-item-link"})['href']
                prc = prod.find("span",{"class":"precio"}).get_text()
                
                dataset.append([desc, url, img, prc])
            
            if(next_page):
                page_content = get_html(next_page['href'], False)
            else:
                break

    df = pd.DataFrame(dataset, columns = headings)
    return df

def insert_data(df):
    server = '.\SQL2019' 
    database = 'Arquisoft' 
    username = 'sa' 
    password = 'sa' 
    cnxn = pyodbc.connect('DRIVER={SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
    cursor = cnxn.cursor()

    cursor.execute("DELETE FROM VendorMaterials")
    # Insert Dataframe into SQL Server:
    for index, row in df.iterrows():
        cursor.execute("INSERT INTO VendorMaterials (Description,SiteURL,ImgURL, Price) values(?,?,?,?)", row.description, row.url, row.image, row.price)
    cnxn.commit()
    cursor.close()


df = process_data()
insert_data(df)
# df.to_csv(CSV_PATH, sep=SEPARATOR, index=False, encoding='utf-8-sig')