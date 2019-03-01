# Google Bigtable AddIn for Microsoft Excel
This Project is basically build to read/write data to Google Cloud Bigtable Using Microsoft Excel.

# Getting Started
In order to Perform the operation Please run the project and you will find "Google CBT" in excel-sheet.
Please find link for creating project and related setting in [Google Cloud](https://cloud.google.com/bigtable/docs/creating-instance)

## Functionality
You can perform below operation using this project:
1. Add Table
2. Insert Test Data
3. Display Specific Table Data
4. Check Table is Exists or not
5. Delete Table


## Prerequisites
In order to build and run this Project you will need to have Microsoft Visual Studio 2017 and Microsoft Office 2013 or Above installed.

## Deployment
Please find Below Steps to Publish Setup of Microsoft Excel Add-in
1. Download Source from GitHub Repository
2. Open in VS 2017 and Build the Project
3. Right Click on GoogleBigTableAddIn Project -> Publish
4. Specify the Location -> Next
5. Select "From a CD-ROM or DVD-ROM" -> Next
6. Click on Publish.

## Installing
Please find Below Steps to Install this AddIn in Microsoft Excel:
1. Right Click on Setup.exe -> Digital Signatures -> View Certificate -> Install Certificate
2. Open Microsoft Excel and Perform Below Setups:
   1. File -> Option -> Add-ins
   2. Select COM Add-ins Under Manage -> Go
   3. Add ->Select setup.exe ->Install ->Ok
   4. Close the current Microsoft Excel and Open New, you will find "Google CBT" on Right side.
   
## Technical Specification
Project is Build in asp.net technology.
