#include "define/stdafx.h"
#include "api/xor.h"
#include "api/api.h"
#include "driver/driver.h"
#include "inject/injector.h"
#include "api/drvutils.h"
#include <filesystem>
#include <urlmon.h>
#pragma comment(lib, "urlmon.lib")

int main()
{
	// driver init
	start_driver();
	cout << endl;

	//string text;
	//ifstream file("vrchat.txt");

	//while (getline(file, text)) {
	//	// Output the text from the file
	//	cout << text;
	//}

	//file.close();

	//if (text == "")
	//{
	//	// setup out val
	//	HKEY key;
	//	// this explicitly will expect someone to be on 64 bit windows, who the fuck uses 32 bit windows anymore
	//	RegOpenKeyExW(HKEY_LOCAL_MACHINE, L"SOFTWARE\\WOW6432Node\\Valve\\Steam", 0, KEY_READ, &key);

	//	// setup another out val
	//	long datasize = MAX_PATH;
	//	TCHAR path[MAX_PATH];
	//	RegQueryValueA(key, "InstallPath", path, &datasize);

	//	string vrc_path = basic_string<TCHAR>(path) + "\\steamapps\\common\\VRChat";
	//	if (std::filesystem::is_directory(vrc_path) == false)
	//	{
	//		cout << "failed to find vrchat directory, please enter your vrchat directory." << std::endl;

	//		enter: {
	//		vrc_path = "";
	//		cin >> vrc_path;
	//		}
	//			
	//		if (std::filesystem::is_directory(vrc_path) == false)
	//		{
	//			cout << "not a valid directory." << std::endl;
	//			goto enter;
	//		}

	//		// could of just changed top level var {file} to an fstream to read/write, but /shrug
	//		ofstream write_file;
	//		write_file.open("vrchat.txt");
	//		write_file << vrc_path;
	//		write_file.close();
	//	}
	//	
	//	text = vrc_path;
	//}

	//URLDownloadToFile(0, "https://ares-mod.com/client/VRCLoader.dll", (text + "\\VRCLoader.dll").c_str(), 0, NULL);
	//URLDownloadToFile(0, "https://ares-mod.com/client/clr_loader.dll", "clr_loader.dll", 0, NULL);

	face_injecor_v2(xor_a("UnityWndClass"), xor_w(L"clr_loader.dll"));

	/*remove("clr_loader.dll");*/

	cout << endl;
	system("pause");
}