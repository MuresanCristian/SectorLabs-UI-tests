Feature: SearchForProperty
	As a airBnb user
	When I search for properties using filters
	Then Properties are properly returned

Background:
	Given User navigates to airbnb.com
	When User search for property using data
		| Key       | Value                    |
		| Where     | Rome, Italy              |
		| Check in  | CurrentDate + "7" days   |
		| Check out | Check in + "7" days      |
		| Who       | "2 adults", "1 children" |

Scenario: Verify that the results match the search criteria
	Then Verify that filters are applied correctly
	And Verify properties match the guests request

Scenario: Verify that the results and details page match the extra filters
	When User enters extra filters
		| Key        | Value |
		| Bedrooms   | 5     |
		| Facilities | Pool  |
	Then Verify that extra filters are applied correctly
	And Verify first property details match the extra options

Scenario: Verify that a property is displayed on the map correctly
	When User hover over first property from the list
	And User click on property on the map
	Then Verify that details from map popup and property list match