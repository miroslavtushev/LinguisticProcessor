# LinguisticProcessor

![](https://github.com/miroslavtushev/LinguisticProcessor/blob/master/img/1.png) ![](https://github.com/miroslavtushev/LinguisticProcessor/blob/master/img/2.png) ![](https://github.com/miroslavtushev/LinguisticProcessor/blob/master/img/3.png)

Developed and used in the following papers:

*M. Tushev, S. Khatiwada and A. Mahmoud, **"Linguistic Change in Open Source Software,"** 2019 IEEE International Conference on Software Maintenance and Evolution (ICSME), Cleveland, OH, USA, 2019, pp. 296-300, doi: 10.1109/ICSME.2019.00045.*
[link](https://ieeexplore.ieee.org/abstract/document/8918934)

***Linguistic Documentation of Software History**
M. Tushev and A. Mahmoud, Inter. Conf. on Program Comprehension (ICPC), 2020.*
[link](https://conf.researchr.org/track/icpc-2020/icpc-2020-era?track=ICPC%20ERA#event-overview)

The program grew from a tiny script to a full-blown implementation with UI over the year or so. Inevitably, it contains a lot of bugs and dead code since I was the only maintainer of the code base. 

**If you are planning on using  this program, add your github access token to Authenticator.cs file!**

## Features
- Extract top-n projects (links) based on the star-rating and programming language from Github
- Download all and (or) missing releases for a project(s)
- Analyze the releases using metrics from the paper (words added, words removed, linguistic change)
- Collect statistics on the project(s): Github statistics (stars, forks, contributors, etc) and local statistics (kLOC, number of releases, linguistic change metrics, etc)

## Implementation
As stated in the paper, Linguistic Processor uses regular expressions to extract identifiers from code. While being less effective in terms of precision than ASTs, regular expressions can work even with incomplete code. The resulted identifiers were then split into their constituent words. We used several splitters: camel case, snake case, space,  etc. In addition, we utilized an algorithm to split words even without delimiter, such as *mainwindow*. The algorithm is based on Zipf's Law and is described [here](https://stackoverflow.com/questions/8870261/how-to-split-text-without-spaces-into-list-of-words). We also did the splitting based on abbreviations, such as "id_form" (id is an abbreviation here).

The program is parallelized as much as possible and where it is appropriate, so it is quite efficient on all modern CPUs (hopefully).
