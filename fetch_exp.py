import urllib.request
import urllib.error

url = 'https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/64473556'
req = urllib.request.Request(
    url, 
    data=None, 
    headers={
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36',
        'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8',
        'Accept-Language': 'en-US,en;q=0.5',
        'Connection': 'keep-alive',
        'Upgrade-Insecure-Requests': '1',
    }
)

try:
    with urllib.request.urlopen(req) as response:
        html = response.read().decode('utf-8')
        with open('c:\\Projetos\\digivice\\AI\\gamefaqs_raw.html', 'w', encoding='utf-8') as f:
            f.write(html)
        print("Success! Saved to gamefaqs_raw.html")
except urllib.error.URLError as e:
    print(f"Failed: {e}")
