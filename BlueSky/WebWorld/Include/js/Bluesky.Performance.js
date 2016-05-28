(function(perf) {
    if (!window.performance) {
        //alert("Your browser don't support HTML5!");
        return;
    }
    perf.performance = window.performance;
    perf.postIndex = -1;
    //在Document完成加载前定时POST界面中所有资源的性能参数
    perf.postResource = function() {
        var entries = perf.performance.getEntries(), length = entries.length;
        if (length == 0 || length - 1 <= postIndex) {
            return;
        }
        var doEntries = [];
        //复制数组中的每个元素构造新的元素数组
        for (var i = postIndex + 1; i < length; i++) {
            doEntries.push(Bluesky.extend(false, {}, entries[i]));
        }
        if (doEntries.length >= 1) {
            Bluesky.Ajax({
                type: "POST",
                url: "Server/SystemManage/WebPerformanceReceiver.ashx?type=resource",
                data: Bluesky.stringify(doEntries)
            });
            perf.postIndex = length - 1;
        }
    }
    //在Document完成加载加载后POST界面整体性能参数
    perf.postPage = function() {
        //var timmings = Bluesky.extend(false, {}, window.performance.timing);
        var timmings = window.performance.timing;
        if (!timmings) {
            return;
        }
        Bluesky.Ajax({
            type: "POST",
            url: "Server/SystemManage/WebPerformanceReceiver.ashx?type=page",
            data: Bluesky.stringify(timmings)
        });
    }
    //perf.submitInterval = setInterval(perf.postResource, 10);
    perf.submitInterval = setInterval(function() { }, 10);
    Bluesky.ready(function() {
        //perf.postResource();
        perf.postPage();
        clearInterval(perf.submitInterval);
    });
})(Bluesky.Performance = Bluesky.Performance || {});