function burnup(trend, stories, from, to) {

    var self = {};
    self.render = render;
    self.velocity = velocity;
    return self;

    function render(element) {
        Chart.Line(element, {
            data: chartData(counts(stories, from, to)),
            options: {
                showLines: true,
                bezierCurve: false
            }
        });
    }

    function velocity() {
        var data = counts(stories, from, to).map(toDoneCounts);
        return trend(data).velocity();
    }

    function counts(stories, from, to) {
        var lastClosed = lastClosedDate(stories);
        return dates(from, to).map(function (d) {
            return {
                date: d,
                total: totalAsOf(stories, d, lastClosed),
                done: doneAsOf(stories, d, lastClosed)
            }
        });
    }

    function totalAsOf(stories, date, lastClosed) {
        return lastClosed < date ? null : stories.filter(createdOnOrBefore(date)).length;
    }

    function doneAsOf(stories, date, lastClosed) {
        return lastClosed < date ? null : stories.filter(closedOnOrBefore(date)).length;
    }

    function closedOnOrBefore(date) {
        return function (s) {
            return s.closed && s.closed <= date;
        }
    }

    function createdOnOrBefore(date) {
        return function (s) {
            return s.created <= date;
        }
    }

    function lastClosedDate(stories) {
        return Math.max.apply(null, stories
            .filter(function (s) { return s.closed != null; })
            .map(function (s) { return s.closed; }));
    }

    function chartData(counts) {
        var currentNumberOfItems = Math.max.apply(null, counts.filter(notNull).map(toTotals));
        return {
            labels: counts.map(toDates),
            datasets: [
                {
                    label: "Stories",
                    backgroundColor: "Blue",
                    fill: false,
                    pointRadius: 0,
                    borderColor: "Blue",
                    data: counts.map(toTotals)
                },
                {
                    label: "Scope",
                    backgroundColor: "#def",
                    fill: false,
                    pointRadius: 0,
                    borderColor: "#def",
                    data: counts.map(function (c) { return currentNumberOfItems; })
                },
                {
                    label: "Done",
                    backgroundColor: "Green",
                    fill: false,
                    pointRadius: 0,
                    borderColor: "Green",
                    data: counts.map(toDoneCounts)
                },
                {
                    label: "Velocity",
                    backgroundColor: "#ada",
                    fill: false,
                    pointRadius: 0,
                    borderColor: "#ada",
                    data: trend(counts.map(toDoneCounts)).line()
                }
            ]
        };
    }

    function toDates(c) {
        return moment(c.date).format('M/D/YYYY');
    }
    function toTotals(c) {
        return c.total;
    }

    function toDoneCounts(c) {
        return c.done;
    }

    function notNull(p) {
        return p.total != null;
    }

    function dates(from, to) {
        var results = [];
        for (var i = 0; moment(from).add(i, 'days').isSameOrBefore(to) ; i += 14) {
            results.push(moment(from).add(i, 'days').toDate());
        }
        return results;
    }

}