#include "define/stdafx.h"
#include "api/xor.h"
#include "api/api.h"
#include "driver/driver.h"
#include "inject/injector.h"
#include "api/drvutils.h"
#include <filesystem>
#include <urlmon.h>
#include <comdef.h>
#pragma comment(lib, "urlmon.lib")

int main()
{
	start_driver();
	cout << endl;

	string text;
	ifstream file("vrchat.txt");

	while (getline(file, text)) {
		// Output the text from the file
		cout << "saved vrchat dir: " << text << std::endl;
	}

	file.close();

	if (text == "")
	{
		HKEY key;
		// this explicitly will expect someone to be on 64 bit windows, who the fuck uses 32 bit windows anymore
		RegOpenKeyExW(HKEY_LOCAL_MACHINE, L"SOFTWARE\\WOW6432Node\\Valve\\Steam", 0, KEY_READ, &key);

		unsigned long type = REG_SZ, size = 1024;
		char res[1024] = "";
		RegQueryValueEx(key,
			"InstallPath",
			NULL,
			&type,
			(LPBYTE)&res[0],
			&size);
		RegCloseKey(key);

		string vrc_path = basic_string<char>(res) + "\\steamapps\\common\\VRChat";
		std::cout << "potential vrc path: " << vrc_path << std::endl;
		std::cout << "checking...";
		if (std::filesystem::is_directory(vrc_path) == false)
		{
			cout << std::endl << std::endl << "failed to find vrchat directory, please enter your vrchat directory." << std::endl;

			vrc_path = "";
			getline(std::cin, vrc_path);

			const std::filesystem::path path = std::filesystem::u8path(vrc_path);
				
			if (std::filesystem::is_directory(path) == false)
			{
				cout << "not a valid directory." << std::endl;
				system("pause");
				return 0;
			}

			ofstream write_file;
			write_file.open("vrchat.txt");
			write_file << vrc_path;
			write_file.close();
		}
		else { std::cout << " valid!" << std::endl << std::endl; }
		text = vrc_path;
	}

	std::cout << std::endl << "downloading files..." << std::endl;



	auto log_res = URLDownloadToFile(0, "https://ares-mod.com/logger/ares_logger.dll", (text + "\\ares_logger.dll").c_str(), 0, NULL);
	_com_error log_err(log_res);
	LPCTSTR log_msg = log_err.ErrorMessage();

	if (log_res != S_OK) std::cout << "failed to download logger, msg: " << log_msg << std::endl;

	auto ldr_res = URLDownloadToFile(0, "https://ares-mod.com/logger/clr_loader.dll", "clr_loader.dll", 0, NULL);
	_com_error ldr_err(ldr_res);
	LPCTSTR ldr_msg = ldr_err.ErrorMessage();

	if (ldr_res != S_OK) std::cout << "failed to download loader, msg: " << ldr_msg << std::endl;

	auto dep_res = URLDownloadToFile(0, "https://ares-mod.com/logger/json.dll", (text + "\\Newtonsoft.Json.dll").c_str(), 0, NULL);
	_com_error dep_err(dep_res);
	LPCTSTR dep_msg = dep_err.ErrorMessage();

	if (dep_res != S_OK) std::cout << "failed to download dependency, msg: " << dep_msg << std::endl;

	if (log_res == S_OK && ldr_res == S_OK && dep_res == S_OK) std::cout << "successfully downloaded all required files" << std::endl << std::endl;
	else { std::cout << "unable to download a required file, check your internet connection." << std::endl; system("pause"); return 0; }

	char type;
	do
	{
		std::cout << "start vrchat? (this will start it in non-vr mode) y/n" << std::endl;
		std::cin >> type;
	} while (!std::cin.fail() && type != 'y' && type != 'n');

	if (type == 'y') ShellExecuteW(NULL, L"open", L"steam://rungameid/438100", NULL, NULL, 1);
	if (type == 'n') std::cout << "you may start vrchat on your own." << std::endl;

	face_injecor_v2(xor_a("UnityWndClass"), xor_w(L"clr_loader.dll"));

	remove("clr_loader.dll");

	cout << endl;
	system("pause");
}