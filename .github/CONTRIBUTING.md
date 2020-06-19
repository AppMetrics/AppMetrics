## How to contribute to App Metrics

Thanks for taking the time to contribute to App Metrics :+1:

### **Coding Style**

Coding style is enforced using StyleCop. To automate the clean up of most rules, the solution includes a "team-shared" [ReSharper DotSettings](AppMetrics.sln.DotSettings), the ReSharper Code Cleanup profile is named `AppMetrics`.

If your not familiar with ReSharper Code Cleanup, see the [code cleanup docs](https://www.jetbrains.com/help/resharper/2016.3/Code_Cleanup__Running_Code_Cleanup.html) and [settings layer docs](https://www.jetbrains.com/help/resharper/2016.3/Reference__Settings_Layers.html). 

### **Have you found a bug?**

**Ensure the bug was not already reported** by searching on GitHub under [Issues](https://github.com/alhardy/AppMetrics/issues).

If you're unable to find an open issue related to the bug you've found go ahead and [open a new issue](https://github.com/alhardy/AppMetrics/issues/new). Be sure to include:

1. A **title and clear description**
2. As much relevant information as possible including the exact steps to reproduce the bug.
3. If possible provide a **code sample** or **unit test** demonstrating the bug.

### **Did you write a patch that fixes a bug?**

* Open a new [GitHub pull request](https://help.github.com/articles/about-pull-requests/) on the `dev` branch.

* Ensure the pull request description clearly describes the problem and solution. Include the relevant issue number in the commit message e.g. `git commit -m '#1 {message}`

### **Requesting a new feature?**

* Suggest your feature as a [new issue](https://github.com/alhardy/AppMetrics/issues/new) to start a discussion.

### **Contributing to the documentation**

App Metrics documentation is built using [Hugo](https://gohugo.io/documentation/), you can find the github repo [here](https://github.com/AppMetrics/Docs.V2.Hugo) and create a new pull request on the `main` branch.
