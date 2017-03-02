// ReadingMemory.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <Windows.h>
#include <vector>

DWORD pid;
const char* WindowName = "Counter-Strike";
DWORD ammoPoolOffset = 0X0D84D6A4;
int MyAmmo; //Donde ire guardando el valor de mi munición restante
//const char* datatoFound = "20";

char* GetAddressOfData(DWORD pid, const char *data, size_t len)
{
	HANDLE process = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, FALSE, pid);
	if (process)
	{
		SYSTEM_INFO si;
		GetSystemInfo(&si);

		MEMORY_BASIC_INFORMATION info;
		std::vector<char> chunk;
		char* p = 0;
		while (p < si.lpMaximumApplicationAddress)
		{
			try {
				if (VirtualQueryEx(process, p, &info, sizeof(info)) == sizeof(info))
				{
					p = (char*)info.BaseAddress;
					chunk.resize(info.RegionSize);
					SIZE_T bytesRead;

					if (ReadProcessMemory(process, p, &chunk[0], info.RegionSize, &bytesRead))
					{
						for (size_t i = 0; i < (bytesRead - len); ++i)
						{
							if (memcmp(data, &chunk[i], len) == 0)
							{
								return (char*)p + i;
							}
						}
					}
				}
			std::cout << "Buscando en memoria ..." << std::endl;
			}
			catch (const std::bad_alloc &e) {
				std::cerr << e.what();
			}
			p += info.RegionSize;
			
		}
	}
	return 0;
}

int main()
{
	HWND windowHandler = FindWindowA(0, WindowName);

	HWND windowHandler2 = FindWindowA(0, "MiProyecto");
	if (windowHandler2) {
		std::cout << "Estas jodido porque no se ha cambiado" << std::endl;
	}
	else {
		std::cout << "ok no" << std::endl;
	}
	GetWindowThreadProcessId(windowHandler, &pid);
	if (windowHandler)
	{
		std::cout << "Proceso encontrado, tiene el siguiente id ---> " << pid << std::endl;
	}
	else
	{
		std::cout << "No se ha podido encontrar el proceso " << std::endl;
	}
	int i = 0;
	while (i < 10) {
		HANDLE pHandle = OpenProcess(PROCESS_VM_READ, FALSE, pid);
		ReadProcessMemory(pHandle, (LPVOID)ammoPoolOffset, &MyAmmo, sizeof(MyAmmo), 0);
		std::cout << "Ahora mismo me queda esto de munición :" << MyAmmo << std::endl;
		std:Sleep(2000);
		i++;
	}

	/*
	std::cout << "Buscando en memoria el valor -----> " << datatoFound << std::endl;
	char* ret = GetAddressOfData(pid, datatoFound, sizeof(datatoFound));
	if (ret)
	{
		std::cout << "Found: " << (void*)ret << "\n";
	}
	else
	{
		std::cout << "Not found\n";
	}
	*/


	system("Pause");
    return 0;
}




