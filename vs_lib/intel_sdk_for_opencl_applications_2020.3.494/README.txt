Intel(R) System Studio 2020 Readme Files
=============================================================

This article contains installation and launch instructions for:

* Windows* Host for a Linux* or Android* Target
* Windows Host for a Windows Target
* macOS* Host for a Linux Target
* Linux Host for a Linux Target

It also includes these appendixes:

*  Appendix 1 Use the command line to list components available for install
*  Appendix 2 Activation methods supported in silent.cfg


Windows Host for a Linux or Android Target
=============================================================

I. Installing on a Windows host for a Linux or Android target

1)  Unpack the ZIP archive file you downloaded, using Windows Explorer.
    Right-click the ZIP file and select "Extract All..." from the menu.
    The ZIP archive contains a config file, license file and EXE file.
2)  Double-click the EXE file to launch the GUI installer.
3)  Follow the installer instruction screens.

The installer requires a reliable active Internet connection. As it runs it
downloads the tools you selected during the installation phase (specified by
the configuration file).

NOTE: If your Internet connection is slow or unreliable, you may need to
download the "Standalone Offline Installer" instead.

II. Launching Intel System Studio 2020 on a Windows host for a Linux or Android target

1)  From the Start menu, select "Intel System Studio 2020 for Linux and Android
    targets" and then select "Intel(R) System Studio." The Eclipse Launcher
    opens. Create the workspace in the user home directory.
2)  Click OK to launch Intel System Studio.
3)  Follow the instructions on the Getting Started welcome screen.


Windows Host for a Windows Target
=============================================================

I. Installing on a Windows host for Windows target

1)  For an *.exe file, run the *.exe file.
    For example, run system_studio_2020_ultimate_edition.exe.
2)  For a *.zip file, unpack the file. Then, from the unpacked folder, run setup.exe.
    For example, run .\system_studio_2020_ultimate_edition_offline\setup.exe.
3)  Follow the instructions in the installer. Note: To find your serial number,
    check for an email from Intel Registration Center.

II. Launching Intel System Studio 2020 on a Windows host for Windows target

1)  From the Start menu, select "Intel System Studio for Windows target."
2)  Then, select "Documentation and Getting Started with Intel System Studio"
    to open the Get Started Guide.
3)  Open Microsoft Visual Studio*.
4)  Follow the instructions in the Getting Started Guide.


macOS Host for a Linux Target
=============================================================

I. Installing on a macOS host - Graphic User Interface Mode

1)  Download and mount the disk image.
2)  Double-click the install icon to start install.app.
3)  Follow the instructions in the installer.

II. Installing on a macOS host - Command Line Interface Mode

1)  Unpack the installer using command: tar -zxf system_studio_2020_offline.tgz.
2)  Run the installer: ./system_studio_2020_offine/system_studio_2020.app.
3)  Follow the instructions in the installer.

III. Launching Intel System Studio 2020 on macOS host

1)  In your Applications folder, navigate to "Intel System Studio 2020 for
    Linux and Android targets."
2)  Double-click "Intel System Studio." The Eclipse Launcher opens.
3)  Create the workspace in the user home directory.
4)  Click OK to launch Intel System Studio.


Linux Host for a Linux Target
=============================================================

I. Installing on a Linux host - Graphic User Interface mode

1)  Unpack the ZIP archive file you downloaded using $ unzip <archive-name>.
    The ZIP archive contains a config file, license file and a tar.gz file.
2)  Extract the installer from the tar.gz file using $ tar xvf <tar-file-name>.
3)  Locate the install.sh file in the extracted installer folder.
4)  Use $ sudo ./install.sh to launch the GUI installer as root.
    Optionally, use $ ./install.sh to launch the installer as a normal user.
5)  Follow the installer instruction screens.

The installer requires a reliable active Internet connection. As it runs it
downloads the tools you selected during the installation phase (specified by
the configuration file).

NOTE: If your Internet connection is slow or unreliable, you may need to
download the "Standalone Offline Installer", instead.

II. Launching Intel System Studio 2020  on a Linux host

1)  Double-click the "Intel System Studio" desktop shortcut.
2)  The Eclipse Launcher opens. Create the workspace in the user home directory.
3)  Click OK to launch Intel System Studio.

III. Installing on a Linux host - Command Line Interface mode

If your system does not support GUI-mode installation, you can install
Intel System Studio in the Command Line Interface (CLI) mode.

The CLI mode requires you to prepare a configuration file before launching the
installation. You can find a template of this configuration file in the
silent.cfg file included in the installation package. The silent.cfg template
contains all of the supported installation options together with their
descriptions and possible values.

III.1 Configuring the silent.cfg Template

The following list contains the minimal set of changes you have to make to the
silent.cfg template:

1) Accept the conditions of the End User License Agreement (EULA) by
   setting the value of ACCEPT_EULA to accept.

   Example:
      # accept EULA, valid values are: {accept, decline}
      ACCEPT_EULA=accept

2) Specify the product activation type by setting the value of ACTIVATION_TYPE.
   Below are the most commonly used license activation methods:

   ACTIVATION_TYPE={exist_lic, license_server, license_file, serial_number}

   You can find the meaning of each of these values in the section Appendix 1
   Activation Methods below. Note: To find your serial number, check for an
   email from Intel Registration Center.


III.2 Launching Installation in CLI Mode

To launch installation in the silent mode, run the following command, where
silent.cfg is the configuration file that resulted from editing the template:
  $ sudo ./install.sh  -s silent.cfg.


Appendix 1 Use the command line to list components available for install
=========================================================================

You can use the command line option to list components from the package that 
are available for installation.

To list the available components:

  1) Enter this command: $ ./install.sh --list_components
     A list of components that are available to install is displayed, 
     with the following information about each component.

       * Unique identifier (abbreviation with version and architecture)
       * Short description
       * Version

Example of component list

  intel-docker-build-meta-any-2020.0-128__noarch - Docker* based build system, 
    version: 2020.0
  eclipse_linux-1.1.1-1__noarch - Eclipse* IDE, version: 1.1.1
  socwatch_cpu_andr_target_2020-2.6.1-1__noarch - Intel(R) SoC Watch for
    Android* targets, version: 2.6.1
  socwatch_cpu_chr_lin_target_2020-2.6.1-1__noarch - Intel(R) SoC Watch for 
    Linux* targets, version: 2.6.1
  wrl-app-dev-1.1.1-1__noarch - WindRiver Linux Application Development Plugins, 
    version: 1.1.1
  wrl-platform-dev-1.1.1-1__noarch - WindRiver Linux Platform Project 
    Development Plugins, version: 1.1.1

To configure component selection in silent mode:

  1)  Add the unique component identifiers to the COMPONENTS section of the 
      silent configuration file.

EXAMPLE

  # List of components to install, valid values are: {ALL, DEFAULTS, anythingpat}
  COMPONENTS=socwatch_cpu_chr_lin_target_2020-2.6.1-1__noarch;wrl-platform-dev-1.1.1-1__noarch

  NOTE: If a selected component requires another component, the required component 
        will be installed by default even if it is not listed in the COMPONENTs section.
         

APPENDIX 2 Activation Methods Supported in silent.cfg
=============================================================

The activation methods supported in silent.cfg have the following meanings:

 - serial_number: the installer contacts an external Intel activation server

   and confirms the validity of the serial number stored in
   ACTIVATION_SERIAL_NUMBER.

   Example:
      ACTIVATION_TYPE=serial_number
      ...
      # Serial number, valid values are: {snpat}
      # snpat - the serial number pattern (ABCD-01234567)
      ACTIVATION_SERIAL_NUMBER=ABCD-01234567

 - exist_lic: the installer searches for an existing license on your machine.
   The search locations are the environment variable INTEL_LICENSE_FILE
   and the default license directory "/opt/intel/licenses/".

   Example:
      ACTIVATION_TYPE=exist_lic

 - license_file: the installer attempts to use the file specified in
   ACTIVATION_LICENSE_FILE as the license file.

   Example:
      ACTIVATION_TYPE=license_file
      ...
      # License file or license server, valid values are: {filepat}
      # filepat - the file location pattern (/file/location/to/license.lic)
      ACTIVATION_LICENSE_FILE=/file/location/to/license.lic

 - license_server: the installer contacts a floating license server on the
   local network to activate the product.

   Make sure your client is correctly set up for your network including all
   networking, routing, name service, and firewall settings. Ensure that your
   client has direct access to your floating license server and that firewalls
   are set up to allow TCP/IP access for the 2 license server ports.

   The installer takes the license information from INTEL_LICENSE_FILE.
   INTEL_LICENSE_FILE can contain either a server address in the port@hostname
   format or the path to a client license file. You can find a more detailed
   detailed description at
   https://software.intel.com/en-us/articles/licensing-setting-up-the-client-floating-license

   Example:
      ACTIVATION_TYPE=license_server
      ...
      # lspat - the license server address pattern (0123@hostname)
      INTEL_LICENSE_FILE=0123@hostname

 - No license file but you have a serial number: please visit
   https://registrationcenter.intel.com to register your serial number.
   As part of the registration, you will receive an email with an attached
   license file. If your serial number is already registered and you need to
   retrieve a license file, please read the following article:
   https://software.intel.com/en-us/articles/how-do-i-manage-my-licenses