function trend(data) {
    var self = {};
    self.line = line;
    self.velocity = velocity;
    return self;

    function velocity() {
        var points = data.map(toPoints);
        var pointsWithCloseDates = points.filter(function (p, i) { return p[1] != null; });
        sI = slopeAndIntercept(pointsWithCloseDates);
        return sI.slope;
    }

    function line() {
        var points = data.map(toPoints);
        var pointsWithCloseDates = points.filter(function (p, i) { return p[1] != null; });
        sI = slopeAndIntercept(pointsWithCloseDates);
        if (!sI)
            return [];

        return points.map(function (p, i) {
            return sI.slope * points[i][0] + sI.intercept;
        });

    }

    function toPoints(value, index) {
        return [index, value];
    }

    function slopeAndIntercept(points) {
        var rV = {},
            N = points.length,
            sumX = 0,
            sumY = 0,
            sumXx = 0,
            sumYy = 0,
            sumXy = 0;

        if (N < 2) {
            return rV;
        }

        for (var i = 0; i < N; i++) {
            var x = points[i][0],
                y = points[i][1];
            sumX += x;
            sumY += y;
            sumXx += (x * x);
            sumYy += (y * y);
            sumXy += (x * y);
        }
        rV['slope'] = ((N * sumXy) - (sumX * sumY)) / (N * sumXx - (sumX * sumX));
        rV['intercept'] = (sumY - rV['slope'] * sumX) / N;
        rV['rSquared'] = Math.abs((rV['slope'] * (sumXy - (sumX * sumY) / N)) / (sumYy - ((sumY * sumY) / N)));

        return rV;
    }
}