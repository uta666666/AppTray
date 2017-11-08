using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppTray.Models {
    public class ButtonInfo : IEnumerable<KeyValuePair<int, AppInfo>> {
        public ButtonInfo(string directory) {
            _currentPageNo = 1;

            _buttonInfoAllPage = Load(directory);
            if (_buttonInfoAllPage == null) {
                _buttonInfoAllPage = new Dictionary<int, Dictionary<int, AppInfo>>();
                _buttonInfoAllPage.Add(_currentPageNo, new Dictionary<int, AppInfo>());
            }
            _buttonInfo = _buttonInfoAllPage[_currentPageNo];
            _maxPageCount = _buttonInfoAllPage.Max(n => n.Key);
        }

        private Dictionary<int, Dictionary<int, AppInfo>> _buttonInfoAllPage;
        private Dictionary<int, AppInfo> _buttonInfo;
        private int _maxPageCount;
        private int _currentPageNo;

        public Dictionary<int, Dictionary<int, AppInfo>> GetButtonInfoAllPage() {
            return _buttonInfoAllPage;
        }

        public Dictionary<int, Dictionary<int, AppInfo>> Load(string directory) {
            string settingFile = Path.Combine(directory, "settings.json");
            if (!File.Exists(settingFile)) {
                return null;
            }
            var buttonInfos = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, AppInfo>>>(File.ReadAllText(settingFile, Encoding.UTF8));
            foreach (var buttonInfo in buttonInfos) {
                foreach (var appInfo in buttonInfo.Value) {
                    if (string.IsNullOrEmpty(appInfo.Value.AppDisplayName)) {
                        appInfo.Value.AppDisplayName = Path.GetFileNameWithoutExtension(appInfo.Value.FilePath);
                    }
                    appInfo.Value.SetBitmapSource();
                }
            }
            return buttonInfos;
        }

        public void Save(string directory) {
            JsonSerializerSettings setting = new JsonSerializerSettings() {
                Formatting = Formatting.Indented
            };
            var jsonString = JsonConvert.SerializeObject(_buttonInfoAllPage, setting);
            File.WriteAllText(Path.Combine(directory, "settings.json"), jsonString, Encoding.UTF8);
        }

        public void AddFromFile(int buttonNo, string filePath) {
            if (_buttonInfo.ContainsKey(buttonNo)) {
                _buttonInfo[buttonNo] = AppInfoFactory(filePath);
            } else {
                _buttonInfo.Add(buttonNo, AppInfoFactory(filePath));
            }
        }

        private AppInfo AppInfoFactory(string filePath) {
            var ext = Path.GetExtension(filePath);
            if (ext.ToLower() == ".exe") {
                return new AppInfoExe(filePath);
            } else if (ext.ToLower() == ".lnk") {
                return new AppInfoLink(filePath);
            } else if (Directory.Exists(filePath)) {
                return new FolderInfo(filePath);
            } else if (File.Exists(filePath)) {
                return new AppInfoFile(filePath);
            } else {
#if DEBUG
                throw new NotImplementedException();
#endif
            }
        }

        public void Add(int buttonNo, AppInfo appInfo) {
            if (_buttonInfo.ContainsKey(buttonNo)) {
                _buttonInfo[buttonNo] = appInfo;
            } else {
                _buttonInfo.Add(buttonNo, appInfo);
            }
        }

        public void Insert(int buttonNo, AppInfo appInfo) {
            var fromButtonNo = GetButtonNo(appInfo);
        }

        public void Move(int fromPageNo, int fromButtonNo, int toPageNo, int toButtonNo, AppInfo appInfo) {
            _buttonInfoAllPage[fromPageNo].Remove(fromButtonNo);

            var buttonInfoTo = _buttonInfoAllPage[toPageNo];
            if (buttonInfoTo.ContainsKey(toButtonNo)) {
                int index = fromButtonNo;

                Stack<AppInfo> appStack = new Stack<AppInfo>();
                appStack.Push(buttonInfoTo[toButtonNo]);
                buttonInfoTo.Remove(toButtonNo);
                buttonInfoTo[toButtonNo] = appInfo;

                if (fromButtonNo < toButtonNo) {
                    for (int i = toButtonNo - 1; i >= fromButtonNo; i--) {
                        if (buttonInfoTo.ContainsKey(i)) {
                            appStack.Push(buttonInfoTo[i]);
                            buttonInfoTo.Remove(i);
                            continue;
                        }
                        index = i;
                        break;
                    }
                    for (int i = index; i < toButtonNo; i++) {
                        buttonInfoTo[i] = appStack.Pop();
                    }
                } else if (fromButtonNo > toButtonNo) {
                    for (int i = toButtonNo + 1; i <= fromButtonNo; i++) {
                        if (buttonInfoTo.ContainsKey(i)) {
                            appStack.Push(buttonInfoTo[i]);
                            buttonInfoTo.Remove(i);
                            continue;
                        }
                        index = i;
                        break;
                    }
                    for (int i = index; i > toButtonNo; i--) {
                        buttonInfoTo[i] = appStack.Pop();
                    }
                }
            } else {
                buttonInfoTo[toButtonNo] = appInfo;
            }
        }

        public void Move(int fromButtonNo, int toButtonNo, AppInfo appInfo) {
            _buttonInfo.Remove(fromButtonNo);

            if (_buttonInfo.ContainsKey(toButtonNo)) {
                int index = fromButtonNo;

                Stack<AppInfo> appStack = new Stack<AppInfo>();
                appStack.Push(_buttonInfo[toButtonNo]);
                _buttonInfo.Remove(toButtonNo);
                _buttonInfo[toButtonNo] = appInfo;

                if (fromButtonNo < toButtonNo) {
                    for (int i = toButtonNo - 1; i >= fromButtonNo; i--) {
                        if (_buttonInfo.ContainsKey(i)) {
                            appStack.Push(_buttonInfo[i]);
                            _buttonInfo.Remove(i);
                            continue;
                        }
                        index = i;
                        break;
                    }
                    for (int i = index; i < toButtonNo; i++) {
                        _buttonInfo[i] = appStack.Pop();
                    }
                } else if (fromButtonNo > toButtonNo) {
                    for (int i = toButtonNo + 1; i <= fromButtonNo; i++) {
                        if (_buttonInfo.ContainsKey(i)) {
                            appStack.Push(_buttonInfo[i]);
                            _buttonInfo.Remove(i);
                            continue;
                        }
                        index = i;
                        break;
                    }
                    for (int i = index; i > toButtonNo; i--) {
                        _buttonInfo[i] = appStack.Pop();
                    }
                }
            } else {
                _buttonInfo[toButtonNo] = appInfo;
            }
        }

        public int GetButtonNo(AppInfo appInfo) {
            return _buttonInfo.Where(n => n.Value == appInfo).Select(n => n.Key).First();
        }

        public void DeleteAppInfo(int key) {
            if (_buttonInfo.ContainsKey(key)) {
                _buttonInfo.Remove(key);
            }
        }

        public void DeleteAppInfo(AppInfo appInfo) {
            DeleteAppInfo(GetButtonNo(appInfo));
        }

        public void AddPage() {
            _maxPageCount++;
            ChangePage(_maxPageCount);
        }

        public void DeletePage() {
            _buttonInfoAllPage.Remove(_currentPageNo);
            _maxPageCount--;
            int tempPageNo = _currentPageNo - 1;
            if (tempPageNo < 1) {
                tempPageNo = 1;
            }
            ChangePage(tempPageNo);
        }

        public bool NextPage() {
            if (_currentPageNo == _maxPageCount) {
                return false;
            }
            ChangePage(_currentPageNo + 1);
            return true;
        }

        public bool PreviousPage() {
            if (_currentPageNo == 1) {
                return false;
            }
            ChangePage(_currentPageNo - 1);
            return true;
        }

        private void ChangePage(int pageNo) {
            _currentPageNo = pageNo;
            if (!_buttonInfoAllPage.ContainsKey(pageNo)) {
                _buttonInfoAllPage.Add(_currentPageNo, new Dictionary<int, AppInfo>());
            }
            _buttonInfo = _buttonInfoAllPage[_currentPageNo];
        }

        public IEnumerator<KeyValuePair<int, AppInfo>> GetEnumerator() {
            foreach (var b in _buttonInfo) {
                yield return b;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public AppInfo this[int buttonNo] {
            get {
                if (_buttonInfo.ContainsKey(buttonNo)) {
                    return _buttonInfo[buttonNo];
                }
                return null;
            }
        }

        public int PageCount {
            get {
                return _maxPageCount;
            }
        }

        public int CurrentPageNo {
            get {
                return _currentPageNo;
            }
        }
    }
}
