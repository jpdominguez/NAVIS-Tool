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
<li><a href="#Cite">Cite this work</a></li>
</ul>
</p>

<h2 name="GettingStarted">Getting started</h2>
<p align="justify">
The following step-by-step guide will show you how to download, install and start using NAVIS. If you prefer a more user-friendly tutorial, you can watch the NAVIS' Getting Started video by clicking on the next image.
</p>
<p align="center">
<a href="https://www.youtube.com/watch?v=xJ27ZqqyDRo"><img align="center" src="http://img.youtube.com/vi/xJ27ZqqyDRo/0.jpg"></a>
</p>
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
After the file has been downloaded, decompress it. NAVIS executable file (NAVIS.exe) is located at the "NAVIS_LatestBuild" folder. Place this folder wherever you prefer and double-click NAVIS.exe to run this tool. If you get an error when trying to execute NAVIS, please check that the folder from which it is being run contains each of the files that are inside the "NAVIS_LatestBuild" folder (no file has been deleted or moved to another folder) and that your have already installed Microsoft .NET Framework 4.5 or greater in your computer. If the problem persist, please send me an <a href="mailto:jpdominguez@atc.us.es">email</a> explaining the situation.
</p>




<h2 name="Usage">Usage</h2>
<p align="justify">
Double-click on the NAVIS.exe file to run it. Three main sections can be seen when the main window is opened: the menu, the toolbar and the cochleogram section:
<ul align="justify">
<li>The menu is located at the upper side of the window. It allows the user to access each of the NAVIS’s functionalities.</li>
<li>The toolbar is located at the left side of the application. It has some quick buttons (shortcuts) for the most-common NAVIS’s utilities. All of these buttons are shown by default when using NAVIS for the first time. Although, the user can select which of them should appear or not, based on his /her interests. This can be done in the settings, which are deeply explained in the <a href="https://github.com/jpdominguez/NAVIS-Tool/wiki/1.-Settings">NAVIS wiki</a>.</li>
<li>The cochleogram section is the big empty space that can be seen after opening NAVIS. This will be used to display the cochleogram of the audio file (in .aedat format) after we load it.</li>
</ul>
</p>
<p align="justify">
The following picture shows the main window that you should be seeing after running the application.
</p>
<p align="center">
<img align="center" src="https://github.com/jpdominguez/NAVIS-Tool/blob/master/NAVIS/Wiki_Images/MainWindow_menus.png">
</p>
<p align="justify">
The folder “AedatSampleFiles” contains several aedat files that can be used to test the application and its functionalities. These files have been captured using jAER, the USBAERmini2 platform and a NAS (with different configurations: mono, stereo, 32-channel and 64-channel).
</p>
<p align="justify">
In this step-by-step tutorial we will be using an aedat file that was captured using 64-channel binaural NAS: “en_un_lugar_de_la_mancha_aedat”, which contains a stream of events that correspond to a young woman reading the first sentence of the famous Spanish novel The Ingenious Gentleman Don Quixote of La Mancha: “En un lugar de La Mancha”.
</p>
<p align="justify">
Before loading this file, we need to configure NAVIS first. Click on File->Settings in the menu or on the button with the gear symbol in the toolbar. This will open the settings window, which is organized into different tabs. Each of the parameters that appear on every tab of the settings window is explained on the <a href="https://github.com/jpdominguez/NAVIS-Tool/wiki/1.-Settings">“Settings” page on the Wiki</a>. For loading a file we only need to focus on the “Main” tab, specifically on three values: “Channels”, “Audio” and “Address Length”. The file that we will use in this tutorial was captured using a 64-channel binaural NAS and jAER; hence, we will select: “64” as “Channels”, “STEREO” as “Audio” and “32” as “Address Length” (jAER use 32 bits for storing the event’s address information). After this settings have been selected, click on the “Save as default settings” button.
</p>
<p align="center">
<img align="center" src="https://github.com/jpdominguez/NAVIS-Tool/blob/master/NAVIS/Wiki_Images/Settings_step.png">
</p>
<p align="justify">
After the settings have been modified and saved, we are now ready to load our first aedat file. Click on File->”Load Aedat” or on the button with the folder symbol in the left toolbar. Navigate to the folder where the file is and select it (this aedat can be found in the “Stereo->32-bit address length” folder from the AedatSampleFiles folder of the GitHub project). The loading process will now begin. Take into account that the file will take longer to load depending on its size and length. After the file is loaded correctly, its cochleogram will be shown in the cochleogram section that we mentioned before.
</p>
<p align="center">
<img align="center" src="https://github.com/jpdominguez/NAVIS-Tool/blob/master/NAVIS/Wiki_Images/MainWindow_cochleogram.png">
</p>
<p align="justify">
Now that the file is loaded, you can use any of the functionalities that NAVIS provides: sonogram, histogram, average activity, disparity between left and right cochleae, etc. This software also allows to split the original file into different single aedat files both manually and automatically based on the channel’s average activity.
</p>
<p align="justify">
In this tutorial we have introduced you on how to install, configure and start using NAVIS. We have shown how to load aedat files and the basics for further processing of the information using the tools that this software provides. A more detailed explanation of the main NAVIS’ functionalities (sonogram, histogram, average activity, disparity between cochleae, Manual Aedat Splitter and Automatic Aedat Splitter) and how to use them can be found in the <a href="https://github.com/jpdominguez/NAVIS-Tool/wiki">Wiki</a> of this project.
</p>



<h2 name="Contributing">Contributing</h2>
<p align="justify">
New functionalities or improvements to the existing project are welcome. To contribute to this project please follow these guidelines:
<ol align="justify">
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
<ul align="justify">
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

<h2 name="Cite">Cite this work</h2>
<p align="justify">
<b>APA</b>: Dominguez-Morales, J. P., Jimenez-Fernandez, A., Dominguez-Morales, M., & Jimenez-Moreno, G. (2017). NAVIS: Neuromorphic Auditory VISualizer Tool. Neurocomputing, 237, 418-422.
</p>
<p align="justify">
<b>ISO 690</b>: DOMINGUEZ-MORALES, Juan P., et al. NAVIS: Neuromorphic Auditory VISualizer Tool. Neurocomputing, 2017, vol. 237, p. 418-422.
</p>
<p align="justify">
<b>MLA</b>: Dominguez-Morales, Juan P., et al. "NAVIS: Neuromorphic Auditory VISualizer Tool." Neurocomputing 237 (2017): 418-422.
</p>
<p align="justify">
<b>BibTeX</b>: @article{dominguez2017navis,
  title={NAVIS: Neuromorphic Auditory VISualizer Tool},
  author={Dominguez-Morales, Juan P and Jimenez-Fernandez, A and Dominguez-Morales, M and Jimenez-Moreno, G},
  journal={Neurocomputing},
  volume={237},
  pages={418--422},
  year={2017},
  publisher={Elsevier}
}
</p>
