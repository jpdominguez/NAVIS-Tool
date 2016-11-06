# Neuromorphic Auditory VISualizer (NAVIS)

<h2 name="Description">Description</h2>
<p align="justify">
<img align="right" src="https://github.com/jpdominguez/NAVIS-Tool/blob/master/NAVIS/Wiki_Images/navis-logo-128.png">
This software presents diverse utilities to develop the first post-processing layer using the neuromorphic auditory sensors (NAS) information. The NAS used implements a cascade filters architecture in FPGA, imitating the behavior of the basilar membrane and inner hair cells, working with the sound information decomposed into its frequency components as spike streams. The neuromorphic hardware interface Address-Event-Representation (AER) is used to propagate auditory information out of the NAS, emulating the auditory vestibular nerve. Using the packetized information (aedat files) generated with jAER software plus an AER to USB computer interface, NAVIS implements a set of graphs that allows to represent the auditory information as cochleograms, histograms, sonograms, etc. It can also split the auditory information into different sets depending on the activity level of the spike streams. The main contribution of this software tool is its capability to apply complex audio post-processing treatments and representations, which is a novelty for spike-based systems in the neuromorphic community. This software will help neuromorphic engineers to build sets for the training of spiking neural networks (SNN).</p>

<h2>Table of contents</h2>
<p align="justify">
<ul>
<li><a href="#Description">Description</a></li>
<li><a href="#GettingStarted">Getting started</a></li>
<li><a href="#Usage">Usage</a></li>
<li><a href="#Contributing">Contributing</a></li>
<li><a href="#Credits">Credits</a></li>
<li><a href="#License">License</a></li>
</ul>
</p>

<h2 name="GettingStarted">Getting started</h2>
<h3>Prerequisites</h3>
<p align="justify">
NAVIS requires Microsoft .NET Framework 4.5 or greater to be executed. The .NET Framework 4.5 and later versions are not supported on Windows XP, but on Windows Vista, Windows 7 and later versions of Windows.
</p>
<p align="justify">
In addition to Microsoft .NET Framework 4.5 or greater, the NAVIS' Visual Studio project requires <a href="http://www.visualstudio.com">Microsoft Visual Studio</a> to compile the code (NAVIS was programmed using Visual Studio Community 2015). 
</p>
<h3>Installation</h3>
<p align="justify">
To use NAVIS, first you need to download the latest release. This can be done by clicking on the "releases" button on the <a href="https://github.com/jpdominguez/NAVIS-Tool">home page</a> or just by cloning/downloading the repository.
</p>
<p align="center">
<img align="center" src="https://github.com/jpdominguez/NAVIS-Tool/blob/master/NAVIS/Wiki_Images/RM_releases.PNG">
</p>
<p align="justify">
Then, select the latest release (it has a green button next to it containing the text "Latest release") and click on the source code download link.
</p>
<p align="center">
<img align="center" src="https://github.com/jpdominguez/NAVIS-Tool/blob/master/NAVIS/Wiki_Images/RM_releaseslatestRelease.png">
</p>
<p align="justify">
After the file has been downloaded, decompress it. NAVIS executable file (NAVIS.exe) is located on the "NAVIS_LatestBuild" folder. Place this folder wherever you prefer and double-click NAVIS.exe to run this tool. If you get an error when trying to execute NAVIS, please check that the folder from which it is being run contains each of the files that are inside the "NAVIS_LatestBuild" folder (no file has been deleted or moved to another folder) and that your have already installed Microsoft .NET Framework 4.5 or greater in your computer. If the problem persist, please send me an <a href="mailto:jpdominguez@atc.us.es">email</a> explaining the situation.
</p>

</p>
<h2 name="Usage">Usage</h2>
<h2 name="Contributing">Contributing</h2>
<p align="justify">
New functionalities or improvements to the existing project are welcome. To contribute to this project please follow these guidelines:
<ol>
<li> Search previous suggestions before making a new one, as yours may be a duplicate.</li>
<li> Fork the project.</li>
<li> Create a branch.</li>
<li> Commit your changes to improve the project.</li>
<li> Push this branch to your GitHub project.</li>
<li> Open a <a href="https://github.com/jpdominguez/NAVIS-Tool/pulls">Pull Request</a>.</li>
<li> Discuss, and optionally continue committing.</li>
<li> Wait untill the project owner merges or closes the Pull Request.</li>
</ol>
If it is a new feature request (e.g., a new functionality), post an issue to discuss this new feature before you start coding. If the project owner approves it, assign the issue to yourself and then do the steps above.
</p>
<p align="justify">
Thank you for contributing in the NAVIS project!
</p>

<h2 name="Credits">Credits</h2>
<p align="justify">
The authors of the NAVIS' original idea are: Juan P. Dominguez-Morales, Angel F. Jimenez-Fernandez, Manuel J. Dominguez-Morales and Gabriel Jimenez-Moreno.
</p>
<p align="justify">
The authors would like to thank and give credit to:
<ul>
<li>Jiménez-Fernández, A., Cerezuela-Escudero, E., Miró-Amarante, L., Domínguez-Morales, M. J., de Asís Gómez-Rodríguez, F., Linares-Barranco, A., & Jiménez-Moreno, G. (2016). "A binaural neuromorphic auditory sensor for FPGA: a spike signal processing approach". IEEE Transactions on Neural Networks and Learning Systems.Year: 2016, Volume: PP, Issue: 99. Pages: 1 - 15, DOI: 10.1109/TNNLS.2016.2583223.</li>
<li>Delbrück, T., “jAER Open Source Project,” 2007. [Online]. Available: http://sourceforge.net/p/jaer/wiki/Home/.</li>
<li>R. Berner et al., “A 5 Meps $100 USB2.0 Address-Event Monitor-Sequencer Interface,” 2007 IEEE ISCAS, 2007.</li>
<li>Alejandro Linares-Barranco, for his support and contribution.</li>
</ul>
</p>

<h2 name="License">License</h2>

<p align="justify">
This project is licensed under the GPL License - see the <a href="https://raw.githubusercontent.com/jpdominguez/NAVIS-Tool/master/LICENSE">LICENSE.md</a> file for details.
</p>
<p align="justify">
Copyright © 2015 Juan P. Dominguez-Morales<br>  
<a href="mailto:jpdominguez@atc.us.es">jpdominguez@atc.us.es</a>
</p>
[![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue.svg)](http://www.gnu.org/licenses/gpl-3.0)
