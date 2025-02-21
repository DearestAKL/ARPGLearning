using System.Collections.Generic;
using Akari.GfCore;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace GameMain.Runtime
{
	public class PatchWindow : MonoBehaviour
	{
		private GfCompositeDisposable _subscriptions;

		/// <summary>
		/// 对话框封装类
		/// </summary>
		private class MessageBox
		{
			private GameObject _cloneObject;
			private Text _content;
			private Button _btnOK;
			private System.Action _clickOK;

			public bool ActiveSelf
			{
				get { return _cloneObject.activeSelf; }
			}

			public void Create(GameObject cloneObject)
			{
				_cloneObject = cloneObject;
				_content = cloneObject.transform.Find("txt_content").GetComponent<Text>();
				_btnOK = cloneObject.transform.Find("btn_ok").GetComponent<Button>();
				_btnOK.onClick.AddListener(OnClickYes);
			}

			public void Show(string content, System.Action clickOK)
			{
				_content.text = content;
				_clickOK = clickOK;
				_cloneObject.SetActive(true);
				_cloneObject.transform.SetAsLastSibling();
			}

			public void Hide()
			{
				_content.text = string.Empty;
				_clickOK = null;
				_cloneObject.SetActive(false);
			}

			private void OnClickYes()
			{
				_clickOK?.Invoke();
				Hide();
			}
		}


		private readonly List<MessageBox> _msgBoxList = new List<MessageBox>();

		// UGUI相关
		private GameObject _messageBoxObj;
		private Slider _slider;
		private Text _tips;


		void Awake()
		{
			_subscriptions = new GfCompositeDisposable();

			_slider = transform.Find("UIWindow/Slider").GetComponent<Slider>();
			_tips = transform.Find("UIWindow/Slider/txt_tips").GetComponent<Text>();
			_tips.text = "Initializing the game world !";
			_messageBoxObj = transform.Find("UIWindow/MessgeBox").gameObject;
			_messageBoxObj.SetActive(false);

			EventManager.Instance.PatchEvent.OnInitializeFailedEvent.GfSubscribe(OnInitializeFailedEvent).AddTo(_subscriptions);
			EventManager.Instance.PatchEvent.OnPatchStatesChangeEvent.GfSubscribe(OnPatchStatesChangeEvent).AddTo(_subscriptions);
			EventManager.Instance.PatchEvent.OnFoundUpdateFilesEvent.GfSubscribe(OnFoundUpdateFilesEvent).AddTo(_subscriptions);
			EventManager.Instance.PatchEvent.OnDownloadProgressUpdateEvent.GfSubscribe(OnDownloadProgressUpdateEvent).AddTo(_subscriptions);
			EventManager.Instance.PatchEvent.OnPackageVersionUpdateFailedEvent.GfSubscribe(OnPackageVersionUpdateFailedEvent).AddTo(_subscriptions);
			EventManager.Instance.PatchEvent.OnPatchManifestUpdateFailedEvent.GfSubscribe(OnPatchManifestUpdateFailedEvent).AddTo(_subscriptions);
			EventManager.Instance.PatchEvent.OnWebFileDownloadFailedEvent.GfSubscribe(OnWebFileDownloadFailedEvent).AddTo(_subscriptions);
		}

		void OnDestroy()
		{
			_subscriptions?.Dispose();
		}

		//------------------------------------------
		//接收事件
		//------------------------------------------
		private void OnInitializeFailedEvent()
		{
			System.Action callback = () => { EventManager.Instance.PatchEvent.OnUserTryInitialize.Invoke(); };
			ShowMessageBox($"Failed to initialize package !", callback);
		}

		private void OnPatchStatesChangeEvent(string tips)
		{
			_tips.text = tips;
		}

		private void OnFoundUpdateFilesEvent(int totalCount, long totalSizeBytes)
		{
			System.Action callback = () => { EventManager.Instance.PatchEvent.OnUserBeginDownloadWebFiles.Invoke(); };
			float sizeMB = totalSizeBytes / 1048576f;
			sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
			string totalSizeMB = sizeMB.ToString("f1");
			ShowMessageBox($"Found update patch files, Total count {totalCount} Total szie {totalSizeMB}MB", callback);
		}

		private void OnDownloadProgressUpdateEvent(DownloadUpdateData updateData)
		{
			_slider.value = (float)updateData.CurrentDownloadCount / updateData.TotalDownloadCount;
			string currentSizeMB = (updateData.CurrentDownloadBytes / 1048576f).ToString("f1");
			string totalSizeMB = (updateData.TotalDownloadBytes / 1048576f).ToString("f1");
			_tips.text = $"{updateData.CurrentDownloadCount}/{updateData.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
		}

		private void OnPackageVersionUpdateFailedEvent()
		{
			System.Action callback = () => { EventManager.Instance.PatchEvent.OnUserTryUpdatePackageVersion.Invoke(); };
			ShowMessageBox($"Failed to update static version, please check the network status.", callback);
		}

		private void OnPatchManifestUpdateFailedEvent()
		{
			System.Action callback = () => { EventManager.Instance.PatchEvent.OnUserTryUpdatePatchManifest.Invoke(); };
			ShowMessageBox($"Failed to update patch manifest, please check the network status.", callback);
		}

		private void OnWebFileDownloadFailedEvent(DownloadErrorData errorData)
		{
			System.Action callback = () => { EventManager.Instance.PatchEvent.OnUserTryDownloadWebFiles.Invoke(); };
			ShowMessageBox($"Failed to download file : {errorData.FileName},error:{errorData.ErrorInfo}", callback);
		}


		/// <summary>
		/// 显示对话框
		/// </summary>
		private void ShowMessageBox(string content, System.Action ok)
		{
			// 尝试获取一个可用的对话框
			MessageBox msgBox = null;
			for (int i = 0; i < _msgBoxList.Count; i++)
			{
				var item = _msgBoxList[i];
				if (item.ActiveSelf == false)
				{
					msgBox = item;
					break;
				}
			}

			// 如果没有可用的对话框，则创建一个新的对话框
			if (msgBox == null)
			{
				msgBox = new MessageBox();
				var cloneObject = GameObject.Instantiate(_messageBoxObj, _messageBoxObj.transform.parent);
				msgBox.Create(cloneObject);
				_msgBoxList.Add(msgBox);
			}

			// 显示对话框
			msgBox.Show(content, ok);
		}
	}
}