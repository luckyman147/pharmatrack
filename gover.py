import json
import time
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select, WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager

# Your existing mapping
tunisia_regions_governorates = {
    "Nord-Ouest": ["Bizerte", "Beja", "Jendouba", "Le Kef"],
    "Nord-Est": ["Tunis", "Ariana", "Ben Arous", "Manouba", "Nabeul", "Zaghouan"],
    "Centre-Ouest/Intérieur": ["Siliana", "Kasserine", "Kairouan", "Sidi Bouzid"],
    "Centre-Est/Sahel": ["Sousse", "Monastir", "Mahdia", "Sfax"],
    "Sud": ["Gafsa", "Tozeur", "Kebili", "Gabes", "Medenine", "Tataouine"]
}

def get_zone_by_governorate(governorate_name):
    for zone, governorates_list in tunisia_regions_governorates.items():
        if governorate_name.lower() in [g.lower() for g in governorates_list]:
            return zone
    return "Sud"

def fetch_tunisia_zones_structure():
    """Scrape DPM.tn to get the structure of zones -> governorates -> delegations."""
    options = webdriver.ChromeOptions()
    options.add_argument("--headless")
    options.add_argument("--window-size=1920,1080")

    driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()), options=options)
    wait = WebDriverWait(driver, 10)

    zones_structure = {}

    try:
        driver.get("https://dpm.tn/dpm_pharm/asppharm/listgouv.php")

        # Select governorates dropdown
        select_element = wait.until(EC.presence_of_element_located((By.NAME, "cod_gouv")))
        select = Select(select_element)
        gov_options = select.options

        for index in range(len(gov_options)):
            value = gov_options[index].get_attribute("value")
            label = gov_options[index].text.strip()

            if not value:
                continue  # skip "select" default option

            print(f"\n🗺️ Processing governorate: {label}")
            select.select_by_index(index)

            # Click "Continuer"
            continue_button = wait.until(EC.element_to_be_clickable((By.XPATH, "//input[@type='submit' and @value='Continuer']")))
            continue_button.click()

            # Wait for delegation dropdown
            wait.until(EC.url_contains("listedelega.php"))
            delegation_dropdown = wait.until(EC.presence_of_element_located((By.NAME, "cod_del")))
            delegation_select = Select(delegation_dropdown)
            delegation_options = delegation_select.options

            delegations = [
                option.text.strip().lower().capitalize()
                for option in delegation_options
                if option.get_attribute("value") and option.text.strip()
            ]

            # Find the zone for this governorate
            zone_name = get_zone_by_governorate(label)
            if zone_name not in zones_structure:
                zones_structure[zone_name] = {}

            zones_structure[zone_name][label] = [d.capitalize() for d in delegations] if delegations else []
            print(f"✅ Found {len(delegations)} delegations for {label}")

            # Go back to the main page
            driver.back()
            wait.until(EC.presence_of_element_located((By.NAME, "cod_gouv")))
            select_element = driver.find_element(By.NAME, "cod_gouv")
            select = Select(select_element)
            gov_options = select.options

    finally:
        driver.quit()
        # Save structure to JSON
        with open("tunisia_zones_structure.json", "w", encoding="utf-8") as f:
            json.dump(zones_structure, f, ensure_ascii=False, indent=4)
        print("\n✅ Successfully saved Tunisia zones structure to tunisia_zones_structure.json")

    return zones_structure

fetch_tunisia_zones_structure()
