# 🚀 Quick Start: Running the Sample Files

### 📂 What's in this folder?
* **`PdfConfigurator.xlsm`**: The Excel control panel where you manage your files and data.
* **`Quotation.pdf`**: A sample business quotation template with placeholder text.
* **`drawings.pdf`**: A sample engineering drawing with title block annotations.

---

### 🛠️ Step-by-Step Setup

**Step 1: Extract the ZIP file**
If you are viewing this inside a `.zip` file, you **must** extract the entire folder to your computer first (e.g., to your Desktop or Documents folder). The Excel macro cannot communicate with the `.exe` if it is still zipped.

**Step 2: Locate the Application**
1. Find the `FindReplaceFreeText.exe` file (it should be in the main folder, one level up from this Sample folder).
2. Hold `Shift`, right-click on `FindReplaceFreeText.exe`, and select **"Copy as path"**.

**Step 3: Configure Excel**
1. Open **`PdfConfigurator.xlsm`**.
2. **Important:** Click **"Enable Content"** or **"Enable Macros"** in the yellow warning bar at the top of Excel. The tool will not run without this.
3. Locate the `EXE_PATH` cell (in the configuration sheet).
4. Paste the path you copied in Step 2 into this cell. *(Remove the quotation marks `"` if they were pasted).*

**Step 4: Update the File Paths**
Because you extracted these files to your own computer, you need to tell Excel where they are:
1. Look at **Table A (TablePDFs)**.
2. Update the **Source PDF Path** to point to the `Quotation.pdf` and `drawings.pdf` files in this sample folder.
3. Update the **Destination PDF Path** to the folder where you want the finished files saved (e.g., `C:\Users\YourName\Desktop\Output_Quotation.pdf`). *Ensure the destination folder actually exists!*

---

### 🏃‍♂️ Running the Engine

1. Review **Table B (TableReplacements)**. Notice how the IDs match the files in Table A, and see the "Find" vs. "Replace" columns.
2. Click the **Run** button in the Excel UserForm sheet to start the automation.
3. A brief command window will appear as the engine processes the PDFs.
4. Go to your destination folder and open the newly generated PDFs. You will see that all the placeholder text has been replaced with the data from Excel!

---

### 💡 Next Steps
Once you understand how the sample works, you can try creating your own template! Open any PDF editor, add a "Free Text" or "Typewriter" annotation (like `{{MY_TEXT}}`), save it, and add it to the Excel sheet.
