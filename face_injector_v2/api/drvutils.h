#pragma once

void start_driver()
{
	driver().handle_driver();

	if (!driver().is_loaded())
	{
		cout << xor_a("driver initialize...") << endl;
		mmap_driver(22);
	}

	driver().handle_driver();
	driver().is_loaded() ? cout << xor_a("driver initialized!") << endl : cout << xor_a("driver initialize error =<") << endl;

	// fallback incase provider 22 fails.
	if (!driver().is_loaded())
	{
		cout << "driver init retry..." << endl;
		mmap_driver(19);

		driver().handle_driver();
		driver().is_loaded() ? cout << xor_a("driver retry success!") << endl : cout << xor_a("retry failed, make sure your anti-virus is not blocking the vulnerable driver.") << endl;
		if (!driver().is_loaded()) { system("pause"); return; }
	}


}

