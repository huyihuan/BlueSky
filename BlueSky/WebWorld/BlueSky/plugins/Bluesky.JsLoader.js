/*
*
* Bluesky Components JsLoader Library v1.0
* 
* Copyright 2016, Yihuan Hu
*
*/
(function(Bluesky) {
	
	//JsLoader 依赖Bluesky库
	if(!Bluesky) {
		return;
	}
	
	Bluesky.extend({
		loader : function(_args) {
			var loader = new Bluesky.JsLoader(_args);
			loader.load();
		},
		JsLoader : function(_args) {
			//参数处理
			var jsLoader = new Bluesky.extend(true, {}, this);
			if(_args.baseUrl && _args.baseUrl != "") {
				jsLoader.baseUrl = _args.baseUrl;
			}
			
			//loader处理，支持只传 module name 数组类型loaders
			if(_args.loaders && _args.loaders.length > 0) {
				Bluesky.foreach(_args.loaders, function(_loader) {
					var loaderArgs = {};
					if(Bluesky.isString(_loader)) {
						loaderArgs.name = _loader;
						loaderArgs.url = jsLoader.baseUrl + _loader + ".js";
					}
					else if(Bluesky.isPureObject(_loader)) {
						loaderArgs = _loader;
					}
					
					jsLoader.loaders.push(new Bluesky.JsLoader.loader(loaderArgs));
				});
			}
			
			if(_args.onSuccess) {
				jsLoader.onSuccess = _args.onSuccess;
			}
			
			if(_args.onFailed) {
				jsLoader.onFailed = _args.onFailed;
			}
			
			//初始化 loadDocument 
			Bluesky.JsLoader.loadDocument = document;
			Bluesky.JsLoader.jsContainer = document.getElementsByTagName("head")[0];
			
			return jsLoader;
		}
	});
	
	Bluesky.extend(true, Bluesky.JsLoader.prototype, {
		baseUrl : "",
		loaders : [],
		onSuccess : null,
		onFailed : null,
		load : function(){
			if(!this.loaders || this.loaders.length <= 0) {
				return;
			}
			
			var closure = this;
			//按照顺序加载 js
			var len = this.loaders.length;
			for(var i=0; i < len ; i++) {
				var n = i, isLast = (n == len - 1);
				if(!isLast) {
					this.loaders[n].nextLoader = this.loaders[n + 1];
				}
			}
			//加载完成毁掉绑定在最后一个Loader上
			if(this.onSuccess) {
				var suc = this.loaders[len - 1].onSuccess;
				this.loaders[len - 1].onSuccess = function() {
					if(suc) {
						suc();
					}
					closure.onSuccess();
				}
			}
			
			this.loaders[0].load();
		}
	});
	
	Bluesky.extend(true, Bluesky.JsLoader, {
		loader : function(_arg) {
			return Bluesky.extend(true, {}, this, _arg);
		},
		loadDocument : null,
		jsContainer : null
	});
	
	Bluesky.extend(true, Bluesky.JsLoader.loader.prototype, {
		name : "",
		url : "",
		onSuccess : null,
		onFailed : null,
		nextLoader : null,
		load : function() { 
			if(!this.url || this.url == "") {
				if(this.onFailed) {
					this.onFailed();
				}
				return;
			}
			
			//外部 document 以及容器准备
			var doc = Bluesky.JsLoader.loadDocument, jsCon = Bluesky.JsLoader.jsContainer;
			if(!doc || !jsCon) {
				return;
			}
			
			var closure = this;
			
			var script = doc.createElement("script");
			script.type = "text/javascript";
			script.src = this.url;
			if(Bluesky.browser.isIE) {
				//对于ie 支持 onreadystatechange
				script.onreadystatechange = function() {
					var state = script.readyState;
					if(state == "complete" || state == "loaded") {
						if(closure.onSuccess) {
							closure.onSuccess();
						}
						if(closure.nextLoader) {
							closure.nextLoader.load();
						}
					}
				};
			}
			else{
				script.onload = function() {
					if(closure.onSuccess) {
							closure.onSuccess();
					}
					if(closure.nextLoader) {
						closure.nextLoader.load();
					}
				}
			}
			jsCon.appendChild(script);
		}
	});
})(Bluesky);