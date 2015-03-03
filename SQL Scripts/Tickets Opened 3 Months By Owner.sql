SELECT hd_ticket.OWNER_ID as owner, user.USER_NAME,
	count(hd_ticket.ID) as total,
	HD_STATUS.NAME AS status,
	MONTH(hd_ticket.CREATED) as month,
	YEAR(hd_ticket.CREATED) as year
FROM HD_TICKET
	JOIN HD_STATUS ON (HD_STATUS.ID = hd_ticket.HD_STATUS_ID)
	LEFT JOIN USER ON user.ID=hd_ticket.OWNER_ID
WHERE (hd_ticket.HD_QUEUE_ID = 4)
AND (
	(HD_STATUS.NAME not like '%Spam%')
	AND (HD_STATUS.NAME not like '%Server Status Report%')
)
AND (hd_ticket.CREATED >= DATE_SUB(DATE_ADD(last_day(NOW()), INTERVAL 1 DAY), INTERVAL 3 MONTH))
GROUP BY hd_ticket.OWNER_ID, YEAR(hd_ticket.CREATED), MONTH(hd_ticket.CREATED)
ORDER BY YEAR(hd_ticket.CREATED), MONTH(hd_ticket.CREATED), hd_ticket.OWNER_ID