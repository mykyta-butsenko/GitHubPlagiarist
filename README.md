# GitHubPlagiarist

This tool utilizes GitHub API and checks for all public GitHub repositories searching for multiple projects with certain keywords (each keyword is processed in a different thread to achieve parallel searching) using advanced filters such as chosen programming languages, excluded repositories, etc. After getting all repositories that match these criteria, it writes them to a log file with simple formatting. Also, it populates another file - the list of already checked repositories so it won’t write them to the main file another time.
