# PDFSigner

Highly customizable command line utility to sign PDF documents using PKCS #12 certificate files. Perfect to call inline from scripts, batches or old programs that cannot sign PDF files OOB.

Features :

- Customizable
- Configuration is inside SQLite Database
- Portable to all OS

Usage :

- Program certificate configuration 
  ```
  PDFSign.exe --setup
  ```
- Launch signature
  ```
  PDFSign.exe -n MyCertificate -p "pdfpath.pdf"
  ```
  
  ![Demo](PDFSign.gif)
 


