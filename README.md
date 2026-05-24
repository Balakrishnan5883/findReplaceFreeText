# PDF Content Automator (FindReplaceFreeText)

An easy-to-use, Excel-driven automation engine designed to generate dynamic PDF documents. 

Whether you need to generate 100 personalized certificates, dynamic technical quotations, or customized engineering labels, this tool transforms static PDF templates into dynamic documents by simply updating an Excel spreadsheet. **No programming knowledge required.**

---

## 🚀 The Core Concept: "The Power of Templates"

The magic of this tool lies in **Free Text Annotation**. 

Instead of manually editing every PDF, you create a "Master Template" once. In this template, you place unique "Placeholder" text (e.g., `{{CLIENT_NAME}}` or `[[PRICE]]`). 

When you run the automator, it scans your Excel list, finds those placeholders in your PDF, and swaps them with the real data from your spreadsheet.

### 💡 Use Case Examples

*   **Mass Certificate Generation**: Have a list of 500 students and their grades in Excel? Generate 500 uniquely personalized certificates in seconds.
*   **Dynamic Quotations**: Link your Excel pricing formulas to a professional PDF quote. Change a quantity in Excel, and the PDF price, total, and date update automatically.
*   **Engineering Labels**: Generate 2D part labels. Use Excel to calculate dimensions or material types and inject them into a standard technical PDF.

---

## 🛠️ How It Works (Setup Guide)

### 1. Prepare your PDF Template
1.  Open any PDF editor (Adobe Acrobat, Foxit, or even Microsoft Edge).
2.  Use the **"Add Text"** or **"Typewriter"** tool to place a unique text string where you want data to appear.
    *   *Tip: Use unique markers like `[VAR_1]` or `{{NAME}}` so the tool doesn't accidentally replace regular text.*
3.  Save this as your "Master Template."

### 2. Configure the Excel Controller (`PdfConfigurator.xlsm`)
Open the provided Excel file and configure two main tables:

#### **Table A: `TablePDFs` (The Task List)**
This table tells the engine which files to process.
| ID | Source PDF Path | Destination PDF Path |
| :--- | :--- | :--- |
| 1 | `C:\Templates\Cert_Template.pdf` | `C:\Output\Student_JohnDoe.pdf` |

*   **ID**: A unique ID used to link to replacements.
*   **Source PDF Path**: Where your master template is located.
*   **Destination PDF Path**: Where the finished, modified PDF should be saved.

#### **Table B: `TableReplacements` (The Data Injection)**
| ID | Find Text (Placeholder) | Replace Text (New Value) |
| :--- | :--- | :--- |
| 1 | `{{NAME}}` | `John Doe` |
| 1 | `SSDT` | `2023-10-27` |

> [!IMPORTANT]
> **Enable Macros**: When opening `PdfConfigurator.xlsm`, you **must** click "Enable Content" at the top of Excel to allow the automation engine to run.

### 3. Configure the Engine Path
1.  In Excel, find the named range `EXE_PATH`.
2.  Paste the full path to the `FindReplaceFreeText.exe` file here.

---

## ⚠️ The Golden Rules (Read Before Running!)

To prevent data loss or software errors, please follow these rules:

1.  **Avoid the Pipe Character (`|`)**: The system uses the `|` symbol to separate data. **Do not** use this character inside your "File paths", "Find" or "Replace" text (e.g., avoid using `12|05|2023` for dates).
2.  **Exact Match Only**: The text in Excel must match the PDF exactly, including **Capitalization** (e.g., `{{name}}` will NOT find `{{NAME}}`).
3.  **Destination Folder Must Exist**: The tool **cannot** create new folders. You must ensure the folder path in your "Destination PDF Path" column already exists on your computer.
4.  **The Overwrite Risk**: 
    *   **Templates**: Ensure your **Destination Path** is never the same as your **Source Path**, or you will permanently overwrite your master template.
    *   **Outputs**: If you run the process twice with the same destination name, the old file will be overwritten without warning.
5.  **Folder Management**: We recommend creating a dedicated "Output" folder for every batch to keep your generated files organized and prevent accidental overwrites.

---

## ✅ Advantages vs. ⚠️ Limitations

### **Advantages**
*   **Zero Coding**: If you can use Excel, you can use this tool.
*   **Massive Scalability**: Use Excel formulas (like `CONCATENATE` or `VLOOKUP`) to generate 1,000+ unique filenames or text strings instantly.
*   **Rapid Deployment**: Change business rules in Excel, and the PDFs update immediately without re-configuring software.

### **Limitations**
*   **Text Only**: The tool only replaces **text**. It cannot move images, resize shapes, or change the layout.
*   **Font Constraints**: The replacement text will attempt to match the document's font. But it supports only Arial, Courier and Times New Roman.
*   **Engineering Drawings**: This tool **cannot** change the actual geometry or vector lines of a CAD file. It can only change the text annotations/labels.
*   **Text Overlap**: If your "Replace Text" is much longer than your "Find Text," it may overlap with other text in the PDF.

---

## 🔍 Troubleshooting
*   **No changes occurring?** Check that the `ID` in `TableReplacements` matches the `ID` in `TablePDFs`.
*   **Error: Path not found?** Check that your Destination folder exists and that all paths are absolute (e.g., `C:\Documents\...`).
*   **Error in `EXE_PATH`?** Ensure the named range `EXE_PATH` points exactly to the `.exe` file location.

