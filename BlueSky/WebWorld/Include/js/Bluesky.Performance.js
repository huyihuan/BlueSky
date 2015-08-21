(function(perf) {
    if (!window.performance) {
        //alert("Your browser don't support HTML5!");
        return;
    }
    perf.performance = window.performance;
    perf.postIndex = -1;
    //��Document��ɼ���ǰ��ʱPOST������������Դ�����ܲ���
    perf.postResource = function() {
        var entries = perf.performance.getEntries(), length = entries.length;
        if (length == 0 || length - 1 <= postIndex) {
            return;
        }
        var doEntries = [];
        //���������е�ÿ��Ԫ�ع����µ�Ԫ������
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
    //��Document��ɼ��ؼ��غ�POST�����������ܲ���
    perf.postPage = function() {
        var timmings = Bluesky.extend(false, {}, window.performance.timing);
        if (!timmings) {
            return;
        }
        Bluesky.Ajax({
            type: "POST",
            url: "Server/SystemManage/WebPerformanceReceiver.ashx?type=page",
            data: Bluesky.stringify(timmings)
        });
    }
    perf.submitInterval = setInterval(function() {
        //perf.postResource(); 
    }, 10);
    Bluesky.ready(function() {
        //perf.postResource();
        perf.postPage();
        clearInterval(perf.submitInterval);
    });
})(Bluesky.Performance = Bluesky.Performance || {});