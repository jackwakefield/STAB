#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <mscoree.h>
#include <metahost.h>
#include <string>
#include <format>
#include <wrl.h>
#pragma comment(lib, "mscoree.lib")

using Microsoft::WRL::ComPtr;

static constexpr const wchar_t* assemblyPath = L"STAB.dll";
static constexpr const wchar_t* typeName = L"STAB.ModLoader";
static constexpr const wchar_t* methodName = L"Initialize";
static constexpr const wchar_t* argument = L"";

void ShowErrorMessage(const std::wstring& message) {
	MessageBoxExW(NULL, message.c_str(), L"STAB", MB_ICONERROR, 0);
}

static DWORD WINAPI InjectAssembly(LPVOID lpParam) {
	ComPtr<ICLRMetaHost> metaHost;
	ComPtr<ICLRRuntimeInfo> runtimeInfo;
	ComPtr<ICLRRuntimeHost> runtimeHost;

	auto hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, &metaHost);
    if (FAILED(hr)) {
		ShowErrorMessage(L"Failed to create CLR instance.");
		return 1;
	}

	hr = metaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, &runtimeInfo);
	if (FAILED(hr)) {
		ShowErrorMessage(L"Failed to retrieve .NET runtime.");
		return 1;
	}

	hr = runtimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, &runtimeHost);
	if (FAILED(hr)) {
		ShowErrorMessage(L"Failed to get CLR runtime host.");
		return 1;
	}

	DWORD returnValue = 0;
	hr = runtimeHost->ExecuteInDefaultAppDomain(assemblyPath, typeName, methodName, argument, &returnValue);
	if (FAILED(hr)) {
		auto errorMsg = std::format(L"Failed to execute assembly. HRESULT: 0x{0:08X}", static_cast<unsigned>(hr));
		ShowErrorMessage(errorMsg);
		return 1;
	}

    return 0;
}

BOOL APIENTRY DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
    switch (fdwReason) {
        case DLL_PROCESS_ATTACH:
			CreateThread(NULL, 0, InjectAssembly, NULL, 0, NULL);
			break;
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            break;
    }

    return TRUE;
}
