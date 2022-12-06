# GitHubPlagiarist

This tool utilizes GitHub API and checks for all public GitHub repositories searching for multiple projects with certain keywords (each keyword is processed in a different thread to achieve parallel searching) using advanced filters such as chosen programming languages, excluded repositories, etc. After getting all repositories that match these criteria, it writes them to a log file with simple formatting. Also, it populates another file - the list of already checked repositories so it won’t write them to the main file another time.

The input data is located at "InputData" folder; this folder contains internal folders for each cofiguration used for analysis. Each internal folder contains: 
  1) JSON config with keywords, languages, link to the exceptions file, API token used to send requests to GitHub API etc.
  2) .txt-file with exceptions (if any).

The output data is located at "Results" folder; this folder contains internal folders for each cofiguration. Each internal folder contains: 
  1) JSON file with details about found code fragment and its respository it belongs to.
  2) Updated file with exceptions.
