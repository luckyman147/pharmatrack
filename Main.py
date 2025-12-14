import json

from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select, WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
import time
import re

class Pharmacie:
    def __init__(self, name, adress, telephone,Horaire):
        self.name = name
        self.adress = adress
        self.telephone = telephone
        self.Horaire=Horaire
       

    def to_dict(self):
        return {
            "Name": self.name.lower().capitalize(),
            "OwnerName":self.name,
            "Telephone": self.telephone,

            "Horaire":self.Horaire,
            "PharmacieType":"Pharmacie",
            "Location": {
                "Zone": self.adress.Zone.lower().capitalize(),
                "Gouvernerat": self.adress.Gouvernerat.lower().capitalize(),
                "delegation": self.adress.Delegation.lower().capitalize(),
                "Rue": self.adress.rue.lower().capitalize()
            }
        }

tunisia_regions_governorates = {
    "Nord-Ouest": [
        "Bizerte",
        "Beja",
        "Jendouba",
        "Le Kef"
    ],
    "Nord-Est": [
        "Tunis",
        "Ariana",
        "Ben Arous",
        "Manouba",
        "Nabeul",
        "Zaghouan"
    ],
    "Centre-Ouest/Intérieur": [
        "Siliana",
        "Kasserine",
        "Kairouan",
        "Sidi Bouzid"
    ],
    "Centre-Est/Sahel": [
        "Sousse",
        "Monastir",
        "Mahdia",
        "Sfax"
    ],
    "Sud": [
        "Gafsa",
        "Tozeur",
        "Kebili",
        "Gabes",
        "Medenine",
        "Tataouine"
    ]
}

def get_zone_by_governorate(governorate_name):
  
    # Iterate through the dictionary where the key is the zone and the value is the list of governorates
    for zone, governorates_list in tunisia_regions_governorates.items():
        # Check if the input governorate is in the current zone's list
        if governorate_name in governorates_list:
            return zone  # Return the zone name immediately upon finding a match
            
    # If the loop finishes without finding a match
    return "Sud"
def supprimCommune(delegation):
        if not isinstance(delegation, str):
        # Handle non-string input gracefully
            return str(delegation) 
        cleaned_string = re.sub(r'\s*Commune\s*', ' ', delegation, flags=re.IGNORECASE)
    
    # Clean up any residual double spaces and strip leading/trailing spaces
        cleaned_string = ' '.join(cleaned_string.split()).strip()

        return cleaned_string.capitalize ()

class Adress: 
  

    def __init__(self, Gouvernerat,delegation,rue):
        self.Zone = get_zone_by_governorate(Gouvernerat.lower().capitalize())
        self.Gouvernerat = Gouvernerat
        self.Delegation = supprimCommune(delegation=delegation)
        self.rue = rue
   

       

def fetch_pharmacies_on_page(driver, gouvernorat_label, delegation_label,Horaire):
    all_pharmacies = []

    gouvernorat = gouvernorat_label
    delegation = delegation_label
   
    # Find the pharmacy table (second table)
    tables = driver.find_elements(By.TAG_NAME, "table")
    if len(tables) < 2:
        print("No pharmacy table found!")
        pharmacies_rows = []
    else:
        pharmacy_table = tables[1]
        pharmacies_rows = pharmacy_table.find_elements(By.TAG_NAME, "tr")[1:]  # Skip header row

    for row in pharmacies_rows:
        cols = row.find_elements(By.TAG_NAME, "td")
        if len(cols) < 3:
            continue
        name = cols[0].text.strip()
        street = cols[1].text.strip()
        telephone = cols[2].text.strip()

        adress = Adress( gouvernorat, delegation, street)
        pharmacie = Pharmacie(name, adress, telephone,Horaire=Horaire)
        print(pharmacie.to_dict())
        all_pharmacies.append(pharmacie)

    return all_pharmacies

options = webdriver.ChromeOptions()
options.add_argument("--headless")
options.add_argument("--window-size=1920,1080")

driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()), options=options)

wait = WebDriverWait(driver, 10)
filename = "example_page_source.html" 
all_pharmacies = []
# Step 1: Go to main page
driver.get("https://dpm.tn/dpm_pharm/asppharm/listgouv.php")
with open(filename, "w", encoding="utf-8") as file:
        file.write(driver.page_source)
    
print(f"Successfully saved page source to {filename}")


try:
    # Wait and get governorate dropdown
    select_element = wait.until(EC.presence_of_element_located((By.NAME, "cod_gouv")))
    select = Select(select_element)
    gov_options = select.options
    radio_buttons = wait.until(EC.presence_of_all_elements_located((By.NAME, "Zone4")))
    label_map = {0: "Jour", 1: "Nuit"}
    

    # Iterate through the elements
    for index in range(len(radio_buttons)):
        horaire = radio_buttons[index]
        
        # Get the friendly label using the index from the dictionary
        horaire_label = label_map.get(index, "Unknown") 
        
        # Click the radio button
        horaire.click()
        
        # Check if the click was successful (i.e., the button is now selected)
        is_checked = horaire.is_selected()
        
        # Print the desired output using the mapped label
        print(f"Label: {horaire_label}, Value: {horaire.get_attribute('value')}, Checked: {is_checked}")
        print(f"{horaire_label} clicked.")
        for index in range(len(gov_options)):
           
            value = gov_options[index].get_attribute("value")
            label = gov_options[index].text.strip()
            print(f"\nProcessing region: {label} (value={value})")

            # Select governorate
            select.select_by_index(index)
            time.sleep(1)

            # Click "Continuer"
            continue_button = wait.until(
                EC.element_to_be_clickable((By.XPATH, "//input[@type='submit' and @value='Continuer']"))
            )
            continue_button.click()

            # Wait for delegation page to load
            wait.until(EC.url_contains("listedelega.php"))
            print(f"Now on page: {driver.current_url}")
            if (horaire_label=="Jour"):
            # Re-fetch the delegation dropdown — DO NOT reuse old references!
                delegation_dropdown = wait.until(EC.presence_of_element_located((By.NAME, "cod_del")))
            else :
                delegation_dropdown = wait.until(EC.presence_of_element_located((By.NAME, "cod_comm")))
                
            delegation_select = Select(delegation_dropdown)
            delegation_options = delegation_select.options

            for index_del in range(len(delegation_options)):
                value_d = delegation_options[index_del].get_attribute("value")
                label_d = delegation_options[index_del].text.strip()
                print(f"Delegation: {label_d} (value={value_d})")

                # Select the delegation
                delegation_select.select_by_index(index_del)
                time.sleep(1)

                # Click "Continuer" to go to pharmacy page
                del_continue = wait.until(
                    EC.element_to_be_clickable((By.NAME, "CHERCHER2"))
                )
                del_continue.click()

                # Wait for pharmacy page to load
                wait.until(EC.url_contains("listedespharm.php"))
                print(f"Now on page: {driver.current_url}")

            #Todo fetch all pharmacie save it to pharmacie object list
                pharmacies = fetch_pharmacies_on_page(driver, label, label_d,Horaire=horaire_label)
                all_pharmacies.extend(pharmacies)
                # Go back to delegation selection page
                driver.back()
                if (horaire_label=="Jour"):
    
                    wait.until(EC.presence_of_element_located((By.NAME, "cod_del")))

                # Re-fetch delegation dropdown and options again (VERY IMPORTANT!)
                    delegation_dropdown = driver.find_element(By.NAME, "cod_del")
                else:
                    wait.until(EC.presence_of_element_located((By.NAME, "cod_comm")))

                # Re-fetch delegation dropdown and options again (VERY IMPORTANT!)
                    delegation_dropdown = driver.find_element(By.NAME, "cod_comm")
                delegation_select = Select(delegation_dropdown)
                delegation_options = delegation_select.options

            # Go back to governorate selection page
            driver.back()
            wait.until(EC.presence_of_element_located((By.NAME, "cod_gouv")))

            # Rebuild the select object
            select_element = driver.find_element(By.NAME, "cod_gouv")
            select = Select(select_element)
            gov_options = select.options  # re-fetch options after page reload
finally:
    driver.quit()
    if all_pharmacies:
        # Convert the list of Pharmacie objects to a list of dictionaries
        pharmacies_data = [p.to_dict() for p in all_pharmacies]

        # Save the list of dictionaries to a JSON file
        try:
            with open('pharmacies_data.json', 'w', encoding='utf-8') as f:
                json.dump(pharmacies_data, f, ensure_ascii=False, indent=4)
            print(f"\nSuccessfully saved {len(all_pharmacies)} pharmacies to pharmacies_data.json.")
        except IOError as e:
            print(f"Error saving file: {e}")
    else:
        print("\nNo pharmacies were scraped to save.")

